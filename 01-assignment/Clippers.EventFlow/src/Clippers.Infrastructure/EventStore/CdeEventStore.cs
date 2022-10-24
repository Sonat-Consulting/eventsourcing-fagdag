using Clippers.Core.EventStore;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace Clippers.Infrastructure.EventStore
{
    public class CdeEventStore : IEventStore
    {
        private readonly DocumentClient _client;
        private readonly string _eventsContainerName = "events";
        private readonly string _databaseName = "eventsdb";
        private readonly string _viewsContainerName = "views";
        private readonly string _leasesContainerName = "leases";


        public CdeEventStore(string endpointUri, string authKey, string database,
            string container = "events")
        {
            _client = new DocumentClient(new Uri(endpointUri), authKey);
            _databaseName = database;
            _eventsContainerName = container;
            CreateDbAndContainersIfNotExists();
            CreateSpIfNotExists();
        }

        public async Task<IEventStream> LoadStreamAsync(string id)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(_databaseName, _eventsContainerName);

            var queryable = _client.CreateDocumentQuery(uri, new SqlQuerySpec
            {
                QueryText = "SELECT * FROM events e WHERE e.stream.id = @streamId ORDER BY e.stream.version",
                Parameters = new SqlParameterCollection
                {
                    new SqlParameter("@streamId", id)
                }
            }).AsDocumentQuery();

            var version = 0;
            var events = new List<IEvent>();

            while (queryable.HasMoreResults)
            {
                var page = await queryable.ExecuteNextAsync();
                foreach (var item in page)
                {
                    version = item.stream.version;

                    events.Add(DeserializeEvent(item));
                }
            }

            return new EventStream(id, version, events);
        }

        public async Task<bool> AppendToStreamAsync(string streamId, int expectedVersion, IEnumerable<IEvent> events)
        {
            // Serialize events to single JSON array to pass to stored procedure.
            var json = SerializeEvents(streamId, expectedVersion, events);

            // Call store procedure to bulk insert events (only if the expected version matches).
            var uri = UriFactory.CreateStoredProcedureUri(_databaseName, _eventsContainerName, "spAppendToStream");
            var options = new RequestOptions { PartitionKey = new PartitionKey(streamId) };
            var result = await _client.ExecuteStoredProcedureAsync<bool>(uri, options, streamId, expectedVersion, json);

            return result.Response;
        }

        private static string SerializeEvents(string streamId, int expectedVersion, IEnumerable<IEvent> events)
        {
            var items = events.Select(e => new
            {
                id = $"{streamId}:{++expectedVersion}:{e.GetType().Name}",
                stream = new
                {
                    id = streamId,
                    version = expectedVersion
                },
                eventType = e.GetType().Name,
                payload = e
            });

            return JsonConvert.SerializeObject(items);
        }

        private static IEvent DeserializeEvent(dynamic item)
        {
            var eventType =
                Type.GetType(
                    $"Clippers.Core.Haircut.Events.{item.eventType}, Clippers.Core");

            return JObject.FromObject(item.payload).ToObject(eventType);
        }

        private void CreateDbAndContainersIfNotExists()
        {
            try
            {
                var db = _client.CreateDatabaseIfNotExistsAsync(new Database { Id = _databaseName }).Result;
            }
            catch (Exception ex)
            {
                throw new Exception("Have you started the Cosmos Db yet?", ex);
            }


            PartitionKeyDefinition pkDefnEvents = new PartitionKeyDefinition() { Paths = new Collection<string>() { "/stream/id" } };
            _client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(_databaseName),
                new DocumentCollection { Id = _eventsContainerName, PartitionKey = pkDefnEvents },
                new RequestOptions { OfferThroughput = 400, PartitionKey = new PartitionKey("/stream/id") }).Wait();

            PartitionKeyDefinition pkDefnViews = new PartitionKeyDefinition() { Paths = new Collection<string>() { "/id" } };
            _client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(_databaseName),
                new DocumentCollection { Id = _viewsContainerName, PartitionKey = pkDefnViews },
                new RequestOptions { OfferThroughput = 400, PartitionKey = new PartitionKey("/id") }).Wait();

            PartitionKeyDefinition pkDefnLease = new PartitionKeyDefinition() { Paths = new Collection<string>() { "/id" } };
            _client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(_databaseName),
                new DocumentCollection { Id = _leasesContainerName, PartitionKey = pkDefnLease },
                new RequestOptions { OfferThroughput = 400, PartitionKey = new PartitionKey("/id") }).Wait();
        }

        private void CreateSpIfNotExists()
        {
            var spBody = File.ReadAllText(@"..\Clippers.Infrastructure\EventStore\StoredProcedures\spAppendToStream.js");

            var spId = "spAppendToStream";
            var spDefinition = new StoredProcedure
            {
                Id = $"{spId}",
                Body = spBody
            };
            var spLink = UriFactory.CreateStoredProcedureUri(_databaseName, _eventsContainerName, spId);
            try
            {
                _client.ReadStoredProcedureAsync(spLink).Wait();
            }
            catch (Exception)
            {
                _client.CreateStoredProcedureAsync(UriFactory.CreateDocumentCollectionUri(_databaseName, _eventsContainerName), spDefinition).Wait();
            }
        }
    }
}
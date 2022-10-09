using Clippers.Core.EventStore;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Clippers.Infrastructure.EventStore
{
    public class CdeEventStore : IEventStore
    {
        private readonly DocumentClient _client;
        private readonly string _container;
        private readonly string _database;

        public CdeEventStore(string endpointUri, string authKey, string database,
            string container = "events")
        {
            _client = new DocumentClient(new Uri(endpointUri), authKey);
            _database = database;
            _container = container;
            //CreateDbAndContainerIfNotExists();
        }

        public async Task<IEventStream> LoadStreamAsync(string id)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(_database, _container);

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
            var uri = UriFactory.CreateStoredProcedureUri(_database, _container, "spAppendToStream");
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

        private void CreateDbAndContainerIfNotExists()
        {
            _client.CreateDatabaseIfNotExistsAsync(new Database { Id = _database }).Wait();
            _client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(_database),
                new DocumentCollection { Id = _container },
                new RequestOptions { OfferThroughput = 400, PartitionKey = new PartitionKey("id") }).Wait();
        }
    }
}
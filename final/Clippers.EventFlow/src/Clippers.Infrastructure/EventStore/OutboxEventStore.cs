using Clippers.Core.EventStore;
using DotNetCore.CAP;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;

namespace Clippers.Infrastructure.EventStore
{
    public class OutboxEventStore : IEventStore
    {
        private readonly ICapPublisher _publisher;
        private readonly IMongoClient _client;
        private readonly IMongoCollection<BsonDocument> _collection;
        private readonly ILogger _logger;

        public OutboxEventStore(ICapPublisher capPublisher, IMongoClient mongoClient, ILogger<OutboxEventStore> logger)
        {
            _publisher = capPublisher;
            _client = mongoClient;
            var db = _client.GetDatabase("clippers");
            _collection = db.GetCollection<BsonDocument>("events");
            _logger = logger;
        }

        public async Task<bool> AppendToStreamAsync(string streamId, int expectedVersion, IEnumerable<IEvent> events)
        {
            try
            {
                foreach (var @event in events)
                {
                    using (var session = _client.StartTransaction(_publisher, autoCommit: false))
                    {
                        var jsonString = SerializeEvent(streamId, expectedVersion, @event);
                        var document = BsonDocument.Parse(jsonString);
                        //eventstore:
                        await _collection.InsertOneAsync(session, document);

                        //publish:
                        var name = @event.GetType().Name;
                        await _publisher.PublishAsync(name, @event);

                        session.CommitTransaction();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save and publish.", ex);
                return false;
            }
            return true;
        }

        public async Task<IEventStream> LoadStreamAsync(string streamId)
        {
            var streamIdFilter = $"'stream.id': '{streamId}'";
            BsonDocument bsonFilter = new BsonDocument();
            // bsonFilter.AllowDuplicateNames = true;
            try
            {
                bsonFilter = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(streamIdFilter);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            var excludeFilter = Builders<BsonDocument>.Projection.Exclude("_id");
            var filter = Builders<BsonDocument>.Filter.Eq("stream.id", streamId);

            var bsonEventsInStream = await _collection.Find(filter).Project(excludeFilter).ToListAsync();

            var dynamicEventsInStream = bsonEventsInStream.Select(x => x.ToDynamic());

            var version = 0;
            var events = new List<IEvent>();
            foreach (var item in dynamicEventsInStream)
            {
                version = item.stream.version;
                events.Add(DeserializeEvent(item));
            }

            return new EventStream(streamId, version, events);
        }

        private static string SerializeEvent(string streamId, int expectedVersion, IEvent @event)
        {
            var item = new
            {
                id = $"{streamId}:{++expectedVersion}:{@event.GetType().Name}",
                stream = new
                {
                    id = streamId,
                    version = expectedVersion
                },
                eventType = @event.GetType().Name,
                payload = @event
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(item);
        }

        private static IEvent DeserializeEvent(dynamic item)
        {
            var eventType =
                Type.GetType(
                    $"Clippers.Core.Haircut.Events.{item.eventType}, Clippers.Core");

            return JObject.FromObject(item.payload).ToObject(eventType);
        }
    }
}

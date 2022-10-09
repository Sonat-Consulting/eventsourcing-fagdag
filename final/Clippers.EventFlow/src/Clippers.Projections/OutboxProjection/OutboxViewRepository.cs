using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;

namespace Clippers.Projections.OutboxProjection
{
    public class OutboxViewRepository : IViewRepository
    {
        private readonly IMongoClient _client;
        private readonly IMongoCollection<ViewEntity> _collection;
        private readonly ILogger<OutboxViewRepository> _logger;

        public OutboxViewRepository(IMongoClient mongoClient, ILogger<OutboxViewRepository> logger)
        {
            _client = mongoClient;
            var db = _client.GetDatabase("clippers");
            _collection = db.GetCollection<ViewEntity>("views");
            _logger = logger;


        }
        public async Task<View> LoadViewAsync(string name)
        {
            var filter = Builders<ViewEntity>.Filter.Eq(x => x._id, name);

            var result = await _collection.Find(filter).ToListAsync();
            var viewDoc = result.FirstOrDefault();

            if (viewDoc is null)
            {
                return new View();
            }

            return new View(viewDoc.Checkpoints, JObject.Parse(viewDoc.Payload), "");

        }

        public async Task<bool> SaveViewAsync(string name, View view)
        {
            var viewEntity = new ViewEntity
            {
                _id = name,
                Checkpoints = view.PartitionCheckpoints,
                Payload = view.Payload.ToString(),
            };
            try
            {
                var replaceOneResult = await _collection.ReplaceOneAsync(
                    x => x._id == viewEntity._id,
                    viewEntity,
                    new ReplaceOptions { IsUpsert = true });
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to save view to MongoDb in Outbox pattern edition.", ex);
                return false;
            }
            return true;
        }
    }
}

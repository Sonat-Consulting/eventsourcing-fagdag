using Microsoft.Azure.Cosmos;
using Newtonsoft.Json.Linq;

namespace Clippers.EventFlow.Projections.Api
{
    public class ProjectionService : IProjectionService
    {
        public async Task<string> GetViews()
        {
            var client = new CosmosClient("https://localhost:8081",
                    "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");

            var container = client.GetContainer("eventsdb", "views");

            var sqlQueryText = "SELECT * FROM views";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<JObject> resultSet = container.GetItemQueryIterator<JObject>(queryDefinition);
            JArray views = new JArray();
            while (resultSet.HasMoreResults)
            {
                var response = await resultSet.ReadNextAsync();
                if (response != null)
                {
                    foreach (var jObject in response.Resource)
                    {
                        JObject newJObject = new JObject();
                        newJObject.Add("id", jObject.GetValue("id"));
                        newJObject.Add("payload", jObject.GetValue("payload"));
                        views.Add(newJObject);
                    }
                }
            }

            return views.ToString();
        }
    }
}

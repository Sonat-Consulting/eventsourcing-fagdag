using Clippers.Core.EventStore;
using Clippers.Infrastructure.EventStore;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;

namespace Clippers.Test.Integration
{
    [TestClass]
    public class CdeEventStoreTests
    {
        private readonly IServiceProvider _serviceProvider;

        public CdeEventStoreTests()
        {
            _serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton(new CosmosClient("https://localhost:8081",
                    "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="))
                .AddSingleton<IEventStore>(
                    new CdeEventStore(
                        "https://localhost:8081",
                        "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                        "eventsdb")
                )
                .BuildServiceProvider();
        }
    }
}
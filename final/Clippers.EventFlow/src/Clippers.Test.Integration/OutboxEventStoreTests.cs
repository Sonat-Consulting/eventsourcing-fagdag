using Clippers.Core.EventStore;
using Clippers.Core.Haircut.Events;
using Clippers.Core.Haircut.Repository;
using Clippers.Core.Haircut.Services;
using Clippers.Infrastructure.EventStore;
using Clippers.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Savorboard.CAP.InMemoryMessageQueue;

namespace Clippers.Test.Integration
{
    [TestClass]
    public class OutboxEventStoreTests
    {
        private readonly IServiceProvider _serviceProvider;

        public OutboxEventStoreTests()
        {
            _serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<IMongoClient>(new MongoClient("mongodb://localhost:27017"))
                .AddCap(x =>
                        {
                            //x.UseInMemoryStorage();
                            x.UseMongoDB("localhost:27017");
                            x.UseInMemoryMessageQueue();
                        }).Services
                .AddSingleton<IEventStore, OutboxEventStore>()
                .AddScoped<IHaircutRepository, HaircutRepository>()
                .AddScoped<IPurchaseHaircutService, PurchaseHaircutService>()
                .BuildServiceProvider();
        }

        [TestMethod]
        public async Task AppendToStreamAsync_AddEvent_IsStored()
        {
            var eventstore = _serviceProvider.GetService<IEventStore>();
            var haircutId = Guid.NewGuid().ToString();

            var haircutPurchased = new HaircutCreated
            {
                HaircutId = haircutId,
                CustomerId = Guid.NewGuid().ToString(),
                DisplayName = "Jan Thomas",
                CreatedAt = DateTime.Now
            };

            var events = new List<IEvent>
            {
                haircutPurchased,
            };

            var streamId = $"haircut:{haircutId}";

            _ = await eventstore?.AppendToStreamAsync(streamId, 0, events);

            var haircutStarted = new HaircutStarted
            {
                HaircutId = haircutPurchased.HaircutId,
                HairdresserId = Guid.NewGuid().ToString(),
                StartedAt = DateTime.Now
            };


            events = new List<IEvent>
            {
                haircutStarted,
            };
            _ = await eventstore?.AppendToStreamAsync(streamId, 1, events);
        }

        [TestMethod]
        public async Task PurchaseHaircutService_PurchaseHaircut_Ok()
        {
            var sut = _serviceProvider.GetService<IPurchaseHaircutService>();
            var haircutId = Guid.NewGuid().ToString();

            var haircutPurchased = new HaircutCreated
            {
                HaircutId = haircutId,
                CustomerId = Guid.NewGuid().ToString(),
                DisplayName = "Jan Thomas",
                CreatedAt = DateTime.Now
            };

            var res = await sut.CreateHaircut(haircutPurchased);

        }
    }
}
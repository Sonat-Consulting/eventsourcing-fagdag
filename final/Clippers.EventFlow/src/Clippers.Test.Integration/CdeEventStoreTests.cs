using AutoFixture;
using Clippers.Core.EventStore;
using Clippers.Core.Haircut.Events;
using Clippers.Core.Haircut.Models;
using Clippers.Core.Haircut.Repository;
using Clippers.Core.Haircut.Services;
using Clippers.Infrastructure.EventStore;
using Clippers.Infrastructure.Repositories;
using FluentAssertions;
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

            var fixture = new Fixture();
            var haircutPurchased = fixture.Create<HaircutCreated>();

            var res = await sut.CreateHaircut(haircutPurchased);

            res.CreatedAt.Should().Be(haircutPurchased.CreatedAt);
            res.HaircutId.Should().Be(haircutPurchased.HaircutId);
            res.CustomerId.Should().Be(haircutPurchased?.CustomerId);
            res.DisplayName.Should().Be(haircutPurchased?.DisplayName);
            res.HaircutStatus.Should().Be(HaircutStatusType.waiting);
            res.Version.Should().Be(1);
            res.Changes.Count.Should().Be(0);
            res.HairdresserId.Should().BeNull();
            res.StartedAt.Should().BeNull();
            res.CancelledAt.Should().BeNull();
            res.CompletedAt.Should().BeNull();




        }
    }
}
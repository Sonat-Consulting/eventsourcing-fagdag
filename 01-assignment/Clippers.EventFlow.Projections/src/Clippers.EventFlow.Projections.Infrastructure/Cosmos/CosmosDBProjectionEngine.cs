using Clippers.EventFlow.Projections.Core.Interfaces;
using Clippers.EventFlow.Projections.Infrastructure.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.Documents.ChangeFeedProcessor;
using Microsoft.Azure.Documents.ChangeFeedProcessor.PartitionManagement;
using Microsoft.Extensions.Options;

namespace Clippers.EventFlow.Projections.Infrastructure.Cosmos
{
    public class CosmosDBProjectionEngine : ICosmosDBProjectionEngine
    {
        private readonly string _endpointUri;
        private readonly string _authKey;
        private readonly string _database;
        private readonly string _eventContainer;
        private readonly string _leaseContainer;
        private readonly string _viewContainer;
        private readonly List<IProjection> _projections;
        private IHubContext<NotificationHub> _notificationHub;

        private IChangeFeedProcessor _changeFeedProcessor;

        public CosmosDBProjectionEngine(IOptions<CosmosDbProjectionEngineConfig> config, IHubContext<NotificationHub> notificationHub)

        {
            _authKey = config.Value.AuthKey;
            _endpointUri = config.Value.EndpointUri;
            _database = config.Value.Database;
            _eventContainer = config.Value.EventContainer;
            _leaseContainer = config.Value.LeaseContainer;
            _viewContainer = config.Value.ViewContainer;
            _projections = new List<IProjection>();
            _notificationHub = notificationHub;
        }

        public void RegisterProjection(IProjection projection)
        {
            _projections.Add(projection);
        }

        public async Task StartAsync()
        {
            var feedCollectionInfo = new DocumentCollectionInfo
            {
                DatabaseName = _database,
                CollectionName = _eventContainer,
                Uri = new Uri(_endpointUri),
                MasterKey = _authKey
            };

            var leaseCollectionInfo = new DocumentCollectionInfo
            {
                DatabaseName = _database,
                CollectionName = _leaseContainer,
                Uri = new Uri(_endpointUri),
                MasterKey = _authKey
            };

            var viewRepository = new CosmosDBViewRepository(_endpointUri, _authKey, _database);

            var builder = new ChangeFeedProcessorBuilder();
            _changeFeedProcessor = await builder
                .WithHostName("Projections")
                .WithFeedCollection(feedCollectionInfo)
                .WithLeaseCollection(leaseCollectionInfo)
                .WithObserverFactory(new EventObserverFactory(_projections, viewRepository, _notificationHub))
                .WithProcessorOptions(new ChangeFeedProcessorOptions
                {
                    StartFromBeginning = true
                })
                .BuildAsync();

            await _changeFeedProcessor.StartAsync();
        }

        public Task StopAsync()
        {
            return _changeFeedProcessor.StopAsync();
        }
    }
}
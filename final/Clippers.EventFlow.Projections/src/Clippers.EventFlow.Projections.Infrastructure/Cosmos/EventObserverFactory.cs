using Clippers.EventFlow.Projections.Core.Interfaces;
using Clippers.EventFlow.Projections.Infrastructure.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.Documents.ChangeFeedProcessor.FeedProcessing;
using Microsoft.Extensions.Logging.Abstractions;

namespace Clippers.EventFlow.Projections.Infrastructure.Cosmos
{
    public class EventObserverFactory : IChangeFeedObserverFactory
    {
        private readonly List<IProjection> _projections;
        private readonly IViewRepository _viewRepository;
        private IHubContext<NotificationHub> _notificationHub;

        public EventObserverFactory(List<IProjection> projections, IViewRepository viewRepository, IHubContext<NotificationHub> notificationHub)
        {
            _projections = projections;
            _viewRepository = viewRepository;
            _notificationHub = notificationHub;
        }

        public IChangeFeedObserver CreateObserver()
        {
            return new EventObserver(_projections, _viewRepository, NullLogger<EventObserver>.Instance, _notificationHub);
        }
    }
}
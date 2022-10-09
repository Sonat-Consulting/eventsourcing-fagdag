using Clippers.EventFlow.Projections.Core.Interfaces;
using Microsoft.Azure.Documents.ChangeFeedProcessor.FeedProcessing;

namespace Clippers.EventFlow.Projections.Infrastructure.Cosmos
{
    public class EventObserverFactory : IChangeFeedObserverFactory
    {
        private readonly List<IProjection> _projections;
        private readonly IViewRepository _viewRepository;

        public EventObserverFactory(List<IProjection> projections, IViewRepository viewRepository)
        {
            _projections = projections;
            _viewRepository = viewRepository;
        }

        public IChangeFeedObserver CreateObserver()
        {
            return new EventObserver(_projections, _viewRepository);
        }
    }
}
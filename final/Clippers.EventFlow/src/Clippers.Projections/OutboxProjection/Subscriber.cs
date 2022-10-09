using Clippers.Core.Haircut.Events;
using DotNetCore.CAP;

namespace Clippers.Projections.OutboxProjection
{
    public class Subscriber : ISubscriber, ICapSubscribe
    {
        private readonly List<IProjection> _projections;
        private readonly IViewRepository _viewRepository;

        public Subscriber(IViewRepository viewRepostory)
        {
            _viewRepository = viewRepostory;
            _projections = new List<IProjection>();
        }

        public Task RegisterProjection(IProjection projection)
        {
            _projections.Add(projection);
            return Task.CompletedTask;
        }

        [CapSubscribe("HaircutCreated")]
        public async Task ReceiveHaircutCreated(HaircutCreated haircutCreated, CancellationToken cancellationToken)
        {
            foreach (var projection in _projections)
            {
                if (!projection.CanHandle(haircutCreated))
                {
                    continue;
                }

                var viewName = projection.GetViewName($"haircut:{haircutCreated.HaircutId}", haircutCreated);

                var handled = false;
                while (!handled)
                {
                    var view = await _viewRepository.LoadViewAsync(viewName);
                    dynamic checkpointInfo = new
                    {
                        Id = haircutCreated.HaircutId,
                        Timestamp = haircutCreated.Timestamp,
                    };
                    if (view.IsNewerThanCheckpoint("0", checkpointInfo))
                    {
                        projection.Apply(haircutCreated, view);
                        view.UpdateCheckpoint("0", checkpointInfo);
                        handled = await _viewRepository.SaveViewAsync(viewName, view);
                    }
                    else
                    {
                        // Already handled.
                        handled = true;
                    }

                    if (!handled)
                    {
                        // Oh noos! Somebody changed the view in the meantime, let's wait and try again.
                        await Task.Delay(500);
                    }
                }
            }
        }

    }
}
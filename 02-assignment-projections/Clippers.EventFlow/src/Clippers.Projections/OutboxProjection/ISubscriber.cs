using Clippers.Core.Haircut.Events;

namespace Clippers.Projections.OutboxProjection
{
    public interface ISubscriber
    {
        Task ReceiveHaircutCreated(HaircutCreated haircutCreated, CancellationToken cancellationToken);
        Task RegisterProjection(IProjection projection);
    }
}

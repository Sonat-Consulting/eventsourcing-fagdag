namespace Clippers.Projections.OutboxProjection
{
    public interface ISubscriber
    {
        Task RegisterProjection(IProjection projection);
    }
}

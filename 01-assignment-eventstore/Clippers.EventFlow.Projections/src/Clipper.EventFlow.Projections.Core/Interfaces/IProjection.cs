namespace Clippers.EventFlow.Projections.Core.Interfaces
{
    public interface IProjection
    {
        bool CanHandle(IEvent @event);

        string GetViewName(string streamId, IEvent @event);

        void Apply(IEvent @event, IView view);
    }
}
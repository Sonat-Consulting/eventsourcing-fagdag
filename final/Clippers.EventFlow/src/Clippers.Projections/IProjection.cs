
using Clippers.Core.EventStore;

namespace Clippers.Projections
{
    public interface IProjection
    {
        bool CanHandle(IEvent @event);

        string GetViewName(string streamId, IEvent @event);

        void Apply(IEvent @event, IView view);
    }
}
using Clippers.Core.EventStore;

namespace Clippers.Core.Haircut.Events
{
    public interface IHaircutEventBase : IEventBase
    {
        string HaircutId { get; set; }
    }
}

using Clippers.Core.EventStore;

namespace Clippers.Core.Haircut.Events
{
    public class HaircutEventBase : EventBase, IEventBase
    {
        public string HaircutId { get; set; }
    }
}

using Clippers.Core.EventStore;

namespace Clippers.Core.Haircut.Events
{
    public class HaircutEventBase : EventBase
    {
        public string HaircutId { get; set; }
    }
}

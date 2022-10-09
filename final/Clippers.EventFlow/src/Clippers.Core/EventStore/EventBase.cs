using System.Diagnostics;

namespace Clippers.Core.EventStore
{
    [DebuggerStepThrough]
    public abstract class EventBase : IEvent
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
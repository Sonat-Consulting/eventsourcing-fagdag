using System.Diagnostics;

namespace Clippers.Core.EventStore
{
    [DebuggerStepThrough]
    public abstract class EventBase : IEventBase
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
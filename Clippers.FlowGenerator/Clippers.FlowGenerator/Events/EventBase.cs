using System.Diagnostics;

namespace Clippers.FlowGenerator.Events
{
    [DebuggerStepThrough]
    public abstract class EventBase : IEvent
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
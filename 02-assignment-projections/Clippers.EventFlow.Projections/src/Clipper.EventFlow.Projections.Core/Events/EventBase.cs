using Clippers.EventFlow.Projections.Core.Interfaces;
using System.Diagnostics;

namespace Clippers.EventFlow.Projections.Core.Events
{
    [DebuggerStepThrough]
    public abstract class EventBase : IEvent
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
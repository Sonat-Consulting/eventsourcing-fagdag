namespace Clippers.Core.EventStore
{
    public interface IEventBase : IEvent
    {
        DateTime Timestamp { get; set; }
    }
}

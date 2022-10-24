namespace Clippers.Core.EventStore
{
    public interface IEvent
    {
        DateTime Timestamp { get; }
    }
}
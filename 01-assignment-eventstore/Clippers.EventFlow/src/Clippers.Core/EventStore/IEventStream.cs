namespace Clippers.Core.EventStore
{
    public interface IEventStream
    {
        string Id { get; }
        int Version { get; }
        IEnumerable<IEvent> Events { get; }
    }
}
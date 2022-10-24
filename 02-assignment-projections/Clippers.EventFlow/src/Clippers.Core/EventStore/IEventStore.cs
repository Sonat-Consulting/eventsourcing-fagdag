namespace Clippers.Core.EventStore
{
    public interface IEventStore
    {
        Task<IEventStream> LoadStreamAsync(string streamId);

        Task<bool> AppendToStreamAsync(
            string streamId,
            int expectedVersion,
            IEnumerable<IEvent> events);
    }
}
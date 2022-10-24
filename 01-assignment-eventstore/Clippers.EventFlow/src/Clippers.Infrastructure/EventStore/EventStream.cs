using Clippers.Core.EventStore;

namespace Clippers.Infrastructure.EventStore
{
    public class EventStream : IEventStream
    {
        private readonly List<IEvent> _events;

        public EventStream(string id, int version, IEnumerable<IEvent> events)
        {
            Id = id;
            Version = version;
            _events = events.ToList();
        }

        public string Id { get; }

        public int Version { get; }

        public IEnumerable<IEvent> Events => _events;
    }
}
using Clippers.Core.EventStore;
using Newtonsoft.Json.Linq;

namespace Clippers.Projections
{
    public abstract class Projection<TView> : IProjection
        where TView : new()
    {
        private readonly Dictionary<Type, Action<IEvent, object>> _handlers;

        public Projection()
        {
            _handlers = new Dictionary<Type, Action<IEvent, object>>();
        }

        public bool CanHandle(IEvent @event)
        {
            return _handlers.ContainsKey(@event.GetType());
        }

        public virtual string GetViewName(string streamId, IEvent @event)
        {
            return typeof(TView).Name;
        }

        public void Apply(IEvent @event, IView view)
        {
            var payload = view.Payload.ToObject<TView>();

            var eventType = @event.GetType();
            if (_handlers.TryGetValue(eventType, out var handler))
            {
                handler(@event, payload);

                view.Payload = JObject.FromObject(payload);
            }
        }

        protected void RegisterHandler<TEvent>(Action<TEvent, TView> handler)
            where TEvent : IEvent
        {
            _handlers[typeof(TEvent)] = (e, v) => handler((TEvent)e, (TView)v);
        }
    }
}
using Clippers.Core.EventStore;
using Clippers.Core.Haircut.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Clippers.Core.Haircut.Models
{
    public class HaircutModel
    {
        public string HaircutId { get; set; }
        public string CustomerId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime StartedAt { get; private set; }
        public string DisplayName { get; private set; }
        public string HairdresserId { get; private set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public HaircutStatusType HaircutStatus { get; private set; }
        public int Version { get; }
        public List<IEvent> Changes { get; } = new List<IEvent>();

        public HaircutModel(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                Mutate(@event);
                Version += 1;
            }
        }

        public HaircutModel(string haircutId, string customerId, string displayName, DateTime createdAt)
        {
            
            Apply(new HaircutCreated
            {
                HaircutId = haircutId,
                CustomerId = customerId,
                DisplayName = displayName,
                CreatedAt = createdAt
            });
        }

        public void Start(string hairdresserId, DateTime startedAt)
        {
            if (HaircutStatus != HaircutStatusType.waiting)
            {
                throw new ArgumentException("You can only start waiting customers.");
            }
            Apply(new HaircutStarted
            {
                HaircutId = HaircutId,
                HairdresserId = hairdresserId,
                StartedAt = startedAt,
            });
        }

        private void Mutate(IEvent @event)
        {
            ((dynamic)this).When((dynamic)@event);
        }

        private void Apply(IEvent @event)
        {
            Changes.Add(@event);
            Mutate(@event);
        }

        private void When(HaircutCreated @event)
        {
            HaircutId = @event.HaircutId;
            CustomerId = @event.CustomerId;
            DisplayName = @event.DisplayName;
            CreatedAt = @event.CreatedAt;
            HaircutStatus = HaircutStatusType.waiting;
        }
        private void When(HaircutStarted @event)
        {
            StartedAt = @event.StartedAt;
            HairdresserId = @event.HairdresserId;
            HaircutStatus = HaircutStatusType.serving;
        }


    }

}

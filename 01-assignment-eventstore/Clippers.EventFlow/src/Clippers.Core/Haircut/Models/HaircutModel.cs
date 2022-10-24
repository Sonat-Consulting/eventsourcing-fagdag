using Clippers.Core.EventStore;
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

        private void Mutate(IEvent @event)
        {
            ((dynamic)this).When((dynamic)@event);
        }
    }

}

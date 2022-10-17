using Clippers.Core.EventStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clippers.Core.Haircut.Events
{
    public interface IHaircutEventBase :IEventBase
    {
        string HaircutId { get; set; }
    }
}

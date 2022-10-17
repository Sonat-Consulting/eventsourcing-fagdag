using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clippers.Core.EventStore
{
    public interface IEventBase: IEvent
    {
        DateTime Timestamp { get; set; }
    }
}

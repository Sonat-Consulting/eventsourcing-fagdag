using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clippers.FlowGenerator
{
    public interface IGenerator
    {
        Task Generate();
    }
}

using Clippers.EventFlow.Projections.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clippers.EventFlow.Projections.Core.Projections
{
    public class NumOfHaircutsCreatedView
    {
        public int NumOfHaircutsCreated { get; set; } = 0;
    }
    public class NumOfHaircutsCreatedProjection : Projection<NumOfHaircutsCreatedView>
    {
        public NumOfHaircutsCreatedProjection()
        {
            RegisterHandler<HaircutCreated>(WhenHaircutCreated);
        }
        private void WhenHaircutCreated(HaircutCreated haircutCreated, NumOfHaircutsCreatedView view)
        {
            view.NumOfHaircutsCreated++;
        }
    }
}

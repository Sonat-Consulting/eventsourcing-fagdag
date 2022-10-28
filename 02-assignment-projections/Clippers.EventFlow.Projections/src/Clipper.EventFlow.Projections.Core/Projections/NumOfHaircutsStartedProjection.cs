using Clippers.EventFlow.Projections.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clippers.EventFlow.Projections.Core.Projections
{
    public class NumOfHaircutsStartedView
    {
        public int NumOfHaircutsStarted { get; set; } = 0;
    }
    public class NumOfHaircutsStartedProjection : Projection<NumOfHaircutsStartedView>
    {
        public NumOfHaircutsStartedProjection()
        {
            RegisterHandler<HaircutStarted>(WhenHaircutStarted);
        }
        private void WhenHaircutStarted(HaircutStarted haircutStarted, NumOfHaircutsStartedView view)
        {
            view.NumOfHaircutsStarted++;
        }
    }
}

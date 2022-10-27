using Clippers.EventFlow.Projections.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clippers.EventFlow.Projections.Core.Projections
{
    public class HaircutStatisticsView
    {
        public int NumOfHaircutsCreated { get; set; } = 0;
        public int NumOfHaircutsCancelled { get; set; } = 0;
    }
    public class HaircutStatisticsProjection : Projection<HaircutStatisticsView>
    {
        public HaircutStatisticsProjection()
        {
            RegisterHandler<HaircutCreated>(WhenHaircutCreated);
            RegisterHandler<HaircutCancelled>(WhenHaircutCancelled);
        }
        private void WhenHaircutCreated(HaircutCreated haircutCreated, HaircutStatisticsView view)
        {
            view.NumOfHaircutsCreated++;
        }

        private void WhenHaircutCancelled(HaircutCancelled haircutCancelled, HaircutStatisticsView view)
        {
            view.NumOfHaircutsCancelled++;
        }
    }
}

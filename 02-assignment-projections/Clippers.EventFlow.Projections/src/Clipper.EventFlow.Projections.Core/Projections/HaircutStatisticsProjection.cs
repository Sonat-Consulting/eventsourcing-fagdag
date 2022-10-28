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
        public int HaircutsCreated { get; set; } = 0;
        public int HaircutsStarted { get; set; } = 0;
        public int HaircutsCompleted { get; set; } = 0;
    }
    public class HaircutStatisticsProjection : Projection<HaircutStatisticsView>
    {
        public HaircutStatisticsProjection()
        {
            RegisterHandler<HaircutCreated>(WhenHaircutCreated);
            RegisterHandler<HaircutStarted>(WhenHaircutStarted);
            RegisterHandler<HaircutCompleted>(WhenHaircutCompleted);
        }
        private void WhenHaircutCreated(HaircutCreated haircutCreated, HaircutStatisticsView view)
        {
            view.HaircutsCreated++;
        }

        private void WhenHaircutStarted(HaircutStarted haircutStarted, HaircutStatisticsView view)
        {
            view.HaircutsStarted++;
        }

        private void WhenHaircutCompleted(HaircutCompleted haircutCompleted, HaircutStatisticsView view)
        {
            view.HaircutsCompleted++;
        }
    }
}

using Clippers.EventFlow.Projections.Core.Events;

namespace Clippers.EventFlow.Projections.Core.Projections
{
    public class HaircutStatisticsView
    {
        public int CreatedCount { get; set; }
        public int StartedCount { get; set; }
        public int CompletedCount { get; set; }
        public int CancelledCount { get; set; }
    }
    public class HaircutStatisticsProjection : Projection<HaircutStatisticsView>
    {
        public HaircutStatisticsProjection()
        {
            RegisterHandler<HaircutCreated>(WhenHaircutCreated);
            RegisterHandler<HaircutStarted>(WhenHaircutStarted);
            RegisterHandler<HaircutCompleted>(WhenHaircutCompleted);
            RegisterHandler<HaircutCancelled>(WhenHaircutCancelled);
        }

        private void WhenHaircutCreated(HaircutCreated haircutCreated, HaircutStatisticsView view)
        {
            view.CreatedCount++;
        }

        private void WhenHaircutStarted(HaircutStarted haircutStarted, HaircutStatisticsView view)
        {
            view.StartedCount++;
        }

        private void WhenHaircutCompleted(HaircutCompleted haircutCompleted, HaircutStatisticsView view)
        {
            view.CompletedCount++;
        }

        private void WhenHaircutCancelled(HaircutCancelled haircutCancelled, HaircutStatisticsView view)
        {
            view.CancelledCount++;
        }

    }
}

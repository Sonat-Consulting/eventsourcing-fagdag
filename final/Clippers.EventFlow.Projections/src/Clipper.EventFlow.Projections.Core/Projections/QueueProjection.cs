using Clippers.EventFlow.Projections.Core.Events;

namespace Clippers.EventFlow.Projections.Core.Projections
{
    public class QueueElement
    {
        public string HaircutId { get; set; }
        public string DisplayName { get; set; }
        public string Status { get; set; }
    }
    public class QueueView
    {
        public List<QueueElement> Customers { get; set; } = new List<QueueElement>();
    }
    public class QueueProjection : Projection<QueueView>
    {
        public QueueProjection()
        {
            RegisterHandler<HaircutCreated>(WhenHaircutCreated);
            RegisterHandler<HaircutStarted>(WhenHaircutStarted);
            RegisterHandler<HaircutCompleted>(WhenHaircutCompleted);
            RegisterHandler<HaircutCancelled>(WhenHaircutCancelled);
        }

        private void WhenHaircutCreated(HaircutCreated haircutCreated, QueueView view)
        {
            view.Customers.Add(new QueueElement
            {
                HaircutId = haircutCreated.HaircutId,
                DisplayName = haircutCreated.DisplayName,
                Status = "waiting",
            });

        }

        private void WhenHaircutStarted(HaircutStarted haircutStarted, QueueView view)
        {
            view.Customers.First(x => x.HaircutId == haircutStarted.HaircutId).Status = "serving";
        }

        private void WhenHaircutCompleted(HaircutCompleted haircutCompleted, QueueView view)
        {
            view.Customers.RemoveAll(x => x.HaircutId == haircutCompleted.HaircutId);
        }

        private void WhenHaircutCancelled(HaircutCancelled haircutCancelled, QueueView view)
        {
            view.Customers.RemoveAll(x => x.HaircutId == haircutCancelled.HaircutId);
        }

    }
}

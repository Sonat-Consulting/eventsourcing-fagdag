using Clippers.Core.Haircut.Events;

namespace Clippers.Projections.Projections
{
    public class QueueDictStyleElement
    {
        public DateTime CreatedAt { get; set; }
        public string DisplayName { get; set; }
        public string Status { get; set; }
    }
    public class QueueDictStyleView
    {
        public int WaitingCount { get; set; }
        public int ServingCount { get; set; }
        public Dictionary<string, QueueDictStyleElement> Customers { get; set; } = new Dictionary<string, QueueDictStyleElement>();

    }
    public class QueueDictStyleProjection : Projection<QueueDictStyleView>
    {
        public QueueDictStyleProjection()
        {
            RegisterHandler<HaircutCreated>(WhenHaircutCreated);
            RegisterHandler<HaircutStarted>(WhenHaircutStarted);
            RegisterHandler<HaircutCompleted>(WhenHaircutCompleted);
            RegisterHandler<HaircutCancelled>(WhenHaircutCancelled);
        }

        private void WhenHaircutCreated(HaircutCreated haircutCreated, QueueDictStyleView view)
        {
            view.Customers[haircutCreated.HaircutId] = new QueueDictStyleElement
            {
                CreatedAt = haircutCreated.CreatedAt,
                DisplayName = haircutCreated.DisplayName,
                Status = "waiting",
            };
            view.WaitingCount++;

        }

        private void WhenHaircutStarted(HaircutStarted haircutStarted, QueueDictStyleView view)
        {
            view.Customers[haircutStarted.HaircutId].Status = "serving";
            view.WaitingCount--;
            view.ServingCount++;
        }

        private void WhenHaircutCompleted(HaircutCompleted haircutCompleted, QueueDictStyleView view)
        {
            view.Customers.Remove(haircutCompleted.HaircutId);
            view.ServingCount--;
        }

        private void WhenHaircutCancelled(HaircutCancelled haircutCancelled, QueueDictStyleView view)
        {
            view.Customers.Remove(haircutCancelled.HaircutId);
            view.WaitingCount--;
        }

    }
}

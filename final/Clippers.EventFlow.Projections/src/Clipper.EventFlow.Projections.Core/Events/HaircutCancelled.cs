namespace Clippers.EventFlow.Projections.Core.Events
{
    public class HaircutCancelled : HaircutEventBase
    {
        public DateTime CancelledAt { get; set; }
    }
}

namespace Clippers.EventFlow.Projections.Core.Events
{
    public class HaircutCompleted : HaircutEventBase
    {
        public DateTime CompletedAt { get; set; }
    }
}

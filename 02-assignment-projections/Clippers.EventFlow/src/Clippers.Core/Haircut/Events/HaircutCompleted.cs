namespace Clippers.Core.Haircut.Events
{
    public class HaircutCompleted : HaircutEventBase, IHaircutEventBase
    {
        public DateTime CompletedAt { get; set; }
    }
}

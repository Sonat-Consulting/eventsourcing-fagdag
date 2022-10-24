namespace Clippers.Core.Haircut.Events
{
    public class HaircutCancelled : HaircutEventBase, IHaircutEventBase
    {
        public DateTime CancelledAt { get; set; }
    }
}

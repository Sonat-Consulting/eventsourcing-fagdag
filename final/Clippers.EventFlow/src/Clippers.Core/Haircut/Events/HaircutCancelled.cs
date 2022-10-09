namespace Clippers.Core.Haircut.Events
{
    public class HaircutCancelled : HaircutEventBase
    {
        public DateTime CancelledAt { get; set; }
    }
}

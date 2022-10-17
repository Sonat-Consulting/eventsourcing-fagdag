namespace Clippers.Core.Haircut.Events
{
    public class HaircutStarted : HaircutEventBase, IHaircutEventBase
    {
        public string HairdresserId { get; set; }
        public DateTime StartedAt { get; set; }
    }
}

namespace Clippers.Core.Haircut.Events
{
    public class HaircutStarted : HaircutEventBase
    {
        public string HairdresserId { get; set; }
        public DateTime StartedAt { get; set; }
    }
}

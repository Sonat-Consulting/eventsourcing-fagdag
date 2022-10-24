namespace Clippers.Core.Haircut.Events
{
    public class StartHaircutCommand
    {
        public string HaircutId { get; set; }
        public string HairdresserId { get; set; }
        public DateTime StartedAt { get; set; }
    }
}

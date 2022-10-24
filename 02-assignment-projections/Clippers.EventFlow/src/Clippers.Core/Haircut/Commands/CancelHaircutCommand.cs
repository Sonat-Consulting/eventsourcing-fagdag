namespace Clippers.Core.Haircut.Events
{
    public class CancelHaircutCommand
    {
        public string HaircutId { get; set; }
        public DateTime CancelledAt { get; set; }
    }
}

namespace Clippers.Core.Haircut.Events
{
    public class CompleteHaircutCommand
    {
        public string HaircutId { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}

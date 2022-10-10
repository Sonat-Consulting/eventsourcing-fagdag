namespace Clippers.FlowGenerator.Events
{
    public class HaircutCreated : HaircutEventBase
    {
        public DateTime CreatedAt { get; set; }
        public string CustomerId { get; set; }
        public string DisplayName { get; set; }
    }
}

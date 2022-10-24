namespace Clippers.Core.Haircut.Events
{
    public class HaircutCreated : HaircutEventBase, IHaircutEventBase
    {
        public DateTime CreatedAt { get; set; }
        public string CustomerId { get; set; }
        public string DisplayName { get; set; }
    }
}

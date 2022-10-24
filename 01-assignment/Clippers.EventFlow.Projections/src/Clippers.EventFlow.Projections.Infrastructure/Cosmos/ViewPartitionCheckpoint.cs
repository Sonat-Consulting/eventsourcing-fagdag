namespace Clippers.EventFlow.Projections.Infrastructure.Cosmos
{
    public class ViewPartitionCheckpoint
    {
        public ViewPartitionCheckpoint()
        {
            DocumentIds = new List<string>();
        }

        public DateTime Timestamp { get; set; }

        public List<string> DocumentIds { get; }
    }
}
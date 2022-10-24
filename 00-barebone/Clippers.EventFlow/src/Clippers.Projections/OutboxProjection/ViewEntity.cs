namespace Clippers.Projections.OutboxProjection
{
    public class ViewEntity
    {
        public string _id { get; set; }
        public Dictionary<string, ViewPartitionCheckpoint> Checkpoints { get; set; }
        public string Payload { get; set; }
    }
}

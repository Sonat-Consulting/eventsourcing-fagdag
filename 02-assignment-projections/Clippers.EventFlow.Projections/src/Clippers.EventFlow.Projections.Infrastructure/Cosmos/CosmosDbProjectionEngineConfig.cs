namespace Clippers.EventFlow.Projections.Infrastructure.Cosmos
{
    public class CosmosDbProjectionEngineConfig
    {
        public string EndpointUri { get; set; } = "https://localhost:8081";
        public string AuthKey { get; set; } = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        public string Database { get; set; } = "eventsdb";
        public string EventContainer { get; set; } = "events";
        public string LeaseContainer { get; set; } = "leases";
        public string ViewContainer { get; set; } = "views";
    }
}

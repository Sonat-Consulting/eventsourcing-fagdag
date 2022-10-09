using Clippers.EventFlow.Projections.Core.Projections;
using Clippers.EventFlow.Projections.Infrastructure.Cosmos;
using Microsoft.Extensions.DependencyInjection;

var builder = new ServiceCollection()
    .AddSingleton(new CosmosDBProjectionEngine("https://localhost:8081",
                       "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                        "eventsdb"))
        .AddLogging()
    .BuildServiceProvider();

var projectionEngine = builder.GetService<CosmosDBProjectionEngine>();

if (projectionEngine is null)
{
    throw new NullReferenceException("projectionEngine is null. Aborting.");
}
projectionEngine.RegisterProjection(new NumOfHaircutsCreatedProjection());
projectionEngine.RegisterProjection(new HaircutStatisticsProjection());
projectionEngine.RegisterProjection(new QueueProjection());
projectionEngine.RegisterProjection(new QueueDictStyleProjection());

System.Console.WriteLine("Starting Cosmos Projections ChangeFeed Processor...");
await projectionEngine.StartAsync();
System.Console.WriteLine("Cosmos Projections ChangeFeed Processor started...");

System.Console.WriteLine("Trykk Enter for å avslutte Clippers PROJECTIONS back-end, Console edition...");
System.Console.ReadLine();
await projectionEngine.StopAsync();


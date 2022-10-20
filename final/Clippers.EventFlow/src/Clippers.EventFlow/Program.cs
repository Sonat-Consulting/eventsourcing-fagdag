using Clippers.Core.EventStore;
using Clippers.Core.Haircut.Events;
using Clippers.Core.Haircut.Repository;
using Clippers.Core.Haircut.Services;
using Clippers.Infrastructure.EventStore;
using Clippers.Infrastructure.Repositories;
using Clippers.Projections.OutboxProjection;
using Clippers.Projections.Projections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
//*************** This is injected for CDE Version (CosmosDB)**************
builder.Services.AddSingleton(new CosmosClient("https://localhost:8081",
                    "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="));
builder.Services.AddSingleton<IEventStore>(
    new CdeEventStore(
        "https://localhost:8081",
        "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
        "eventsdb")
);
//************** END CDE Injection *****************************************

//*************** This is injected for Outbox Version (MongoDB)  **************
//builder.Services.AddSingleton<IMongoClient>(new MongoClient("mongodb://localhost:27017")); //Options, If you are using MongoDB
//builder.Services.AddSingleton<ISubscriber, Subscriber>();
//builder.Services.AddCap(x =>
//{
//    //x.UseInMemoryStorage();
//    x.UseMongoDB("localhost:27017");
//    x.UseInMemoryMessageQueue();
//});

//builder.Services.AddSingleton<IEventStore, OutboxEventStore>();
//builder.Services.AddSingleton<IViewRepository, OutboxViewRepository>();
//************** END Outbox Injection *****************************************

builder.Services.AddScoped<ICreateHaircutService, CreateHaircutService>();
builder.Services.AddScoped<IStartHaircutService, StartHaircutService>();
builder.Services.AddScoped<ICompleteHaircutService, CompleteHaircutService>();
builder.Services.AddScoped<ICancelHaircutService, CancelHaircutService>();
builder.Services.AddScoped<IHaircutRepository, HaircutRepository>();


var app = builder.Build();

var subscriber = app.Services.GetService<ISubscriber>();
subscriber?.RegisterProjection(new NumOfHaircutsCreatedProjection());
subscriber?.RegisterProjection(new HaircutStatisticsProjection());
subscriber?.RegisterProjection(new QueueProjection());
subscriber?.RegisterProjection(new QueueDictStyleProjection());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/createHaircut", async ([FromBody] CreateHaircutCommand createHaircutCommand, [FromServices] ICreateHaircutService createHaircutService) =>
{
    var ret = await createHaircutService.CreateHaircut(createHaircutCommand);
    return ret;
});

app.MapPost("/startHaircut", async ([FromBody] StartHaircutCommand startHaircutCommand, [FromServices] IStartHaircutService startHaircutService) =>
{
    var ret = await startHaircutService.StartHaircut(startHaircutCommand);
    return ret;
});

app.MapPost("/completeHaircut", async ([FromBody] CompleteHaircutCommand completeHaircutCommand, [FromServices] ICompleteHaircutService completeHaircutService) =>
{
    var ret = await completeHaircutService.CompleteHaircut(completeHaircutCommand);
    return ret;
});

app.MapPost("/cancelHaircut", async ([FromBody] CancelHaircutCommand cancelHaircutCommand, [FromServices] ICancelHaircutService cancelHaircutService) =>
{
    var ret = await cancelHaircutService.CancelHaircut(cancelHaircutCommand);
    return ret;
});

app.Run();
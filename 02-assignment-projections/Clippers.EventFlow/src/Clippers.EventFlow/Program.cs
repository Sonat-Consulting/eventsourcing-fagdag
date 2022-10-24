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
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
    });
});

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

builder.Services.AddSwaggerGen(opts => opts.EnableAnnotations());

var app = builder.Build();

app.UseCors();

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
}).WithMetadata(new SwaggerOperationAttribute(summary: "Create a haircut in the EventStore.", description: "Creates the haircut. Returns the Haircut with its new HaircutId, in status waiting."));

app.MapPost("/startHaircut", async ([FromBody] StartHaircutCommand startHaircutCommand, [FromServices] IStartHaircutService startHaircutService) =>
{
    try
    {
        var ret = await startHaircutService.StartHaircut(startHaircutCommand);
        return Results.Ok(ret);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex);
    }
}).WithMetadata(new SwaggerOperationAttribute(summary: "Starts a haircut in the EventStore.", description: "Starts the haircut identified by the HaircutId provided. You can only start a haircut when it is in the waiting status. "))
  .Produces<string>(StatusCodes.Status200OK)
  .Produces(StatusCodes.Status400BadRequest);

app.MapPost("/completeHaircut", async ([FromBody] CompleteHaircutCommand completeHaircutCommand, [FromServices] ICompleteHaircutService completeHaircutService) =>
{
    try
    {
        var ret = await completeHaircutService.CompleteHaircut(completeHaircutCommand);
        return Results.Ok(ret);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex);
    }
}).WithMetadata(new SwaggerOperationAttribute(summary: "Completes a haircut in the EventStore.", description: "Completes the haircut identified by the HaircutId provided.  You can only complete a haircut when it is in the serving status."))
  .Produces<string>(StatusCodes.Status200OK)
  .Produces(StatusCodes.Status400BadRequest);

app.MapPost("/cancelHaircut", async ([FromBody] CancelHaircutCommand cancelHaircutCommand, [FromServices] ICancelHaircutService cancelHaircutService) =>
{
    try
    {
        var ret = await cancelHaircutService.CancelHaircut(cancelHaircutCommand);
        return Results.Ok(ret);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex);
    }
    
}).WithMetadata(new SwaggerOperationAttribute(summary: "Cancel a haircut in the EventStore.", description: "Cancel the haircut identified by the HaircutId provided.  You can only cancel a haircut in the waiting status."))
  .Produces<string>(StatusCodes.Status200OK)
  .Produces(StatusCodes.Status400BadRequest);

app.Run();
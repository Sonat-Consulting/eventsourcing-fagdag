using Clippers.Core.EventStore;
using Clippers.Core.Haircut.Commands;
using Clippers.Core.Haircut.Events;
using Clippers.Core.Haircut.Repository;
using Clippers.Core.Haircut.Services;
using Clippers.Infrastructure.EventStore;
using Clippers.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
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


builder.Services.AddScoped<IHaircutRepository, HaircutRepository>();

builder.Services.AddScoped<ICreateHaircutService, CreateHaircutService>();
builder.Services.AddScoped<IStartHaircutService, StartHaircutService>();

builder.Services.AddSwaggerGen(opts => opts.EnableAnnotations());

var app = builder.Build();

app.UseCors();

app.MapPost("/CreateHaircut", async([FromBody] CreateHaircutCommand createHaircutCommand, [FromServices] ICreateHaircutService createHaircutService) =>
{
    return await createHaircutService.CreateHaircut(createHaircutCommand);
});

app.MapPost("/startHaircut", async ([FromBody] StartHaircutCommand startHaircutCommand, [FromServices] IStartHaircutService startHaircutService) =>
{
    try
    {
        var ret = await startHaircutService.StartHaircut(startHaircutCommand);
        return Results.Ok(ret);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.ToString());
    }
}).WithMetadata(new SwaggerOperationAttribute(summary: "Starts a haircut in the EventStore.", description: "Starts the haircut identified by the HaircutId provided. You can only start a haircut when it is in the waiting status. "))
  .Produces<string>(StatusCodes.Status200OK)
  .Produces(StatusCodes.Status400BadRequest);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.Run();
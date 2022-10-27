using Clippers.EventFlow.Projections.Api;
using Clippers.EventFlow.Projections.Core.Projections;
using Clippers.EventFlow.Projections.Infrastructure.Cosmos;
using Clippers.EventFlow.Projections.Infrastructure.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Net.Http.Headers;
using Swashbuckle.AspNetCore.Annotations;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
        builder.AllowCredentials();
        builder.WithHeaders(HeaderNames.AccessControlAllowCredentials, "true");
        builder.WithHeaders(HeaderNames.XRequestedWith);
        builder.WithHeaders("x-signalr-user-agent");
    });
});



builder.Services.AddSingleton(new CosmosClient("https://localhost:8081",
                    "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="));

builder.Services.Configure<CosmosDbProjectionEngineConfig>(builder.Configuration.GetSection("CosmosDbProjectionEngine"));

builder.Services.AddSingleton<ICosmosDBProjectionEngine, CosmosDBProjectionEngine>();
builder.Services.AddScoped<IProjectionService, ProjectionService>();

builder.Services.AddSwaggerGen(opts => opts.EnableAnnotations());

builder.Services.AddSignalR();

var app = builder.Build();

app.UseCors();
app.MapHub<NotificationHub>("/projectionchanged");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/projections", async ([FromServices] IProjectionService projectionService) =>
{
    var result = await projectionService.GetViews();
    return result;
})
    .WithMetadata(new SwaggerOperationAttribute(summary: "Get all Projections", description: "Get all the projections as JSON."))
    .Produces<string>(StatusCodes.Status200OK)
    ;

app.MapGet("/projections/{name}", async (string name, [FromServices] IProjectionService projectionService) =>
{
    var result = await projectionService.GetView(name);

    if(string.IsNullOrEmpty(result) || result == "{}")
    {
        return Results.NotFound();
    }
    return Results.Ok(result);



})
    .WithMetadata(new SwaggerOperationAttribute(summary: "Get a Projection by name", description: "Get a specific projection as JSON."))
    .Produces<string>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    ;

var projectionEngine = app.Services.GetService<ICosmosDBProjectionEngine>();
if (projectionEngine is null)
{
    throw new NullReferenceException("projectionEngine is null. Aborting.");
}
projectionEngine.RegisterProjection(new NumOfHaircutsCreatedProjection());
projectionEngine.RegisterProjection(new HaircutStatisticsProjection());
projectionEngine.RegisterProjection(new QueueProjection());
projectionEngine.RegisterProjection(new QueueDictStyleProjection());

//System.Console.WriteLine("Starting Cosmos Projections ChangeFeed Processor...");
await projectionEngine.StartAsync();

app.Run();

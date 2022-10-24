using Clippers.Core.EventStore;
using Clippers.Infrastructure.EventStore;
using Clippers.Projections.OutboxProjection;
using Microsoft.Azure.Cosmos;

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

builder.Services.AddSwaggerGen(opts => opts.EnableAnnotations());

var app = builder.Build();

app.UseCors();

var subscriber = app.Services.GetService<ISubscriber>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();
using System.Text.Json;
using System.Text.Json.Serialization;
using Atc.Cosmos;
using Microsoft.AspNetCore.Mvc;
using Sample.ExpenseTracker.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var cosmosOptions = new CosmosOptions
{
    DatabaseName = "Sample-ExpenseTracker",
    SerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    },
};

cosmosOptions.UseCosmosEmulator();

builder.Services.ConfigureCosmos(
    cosmosOptions,
    cosmosBuilder =>
    {
        cosmosBuilder
            .AddContainer<SomeResource>(SomeResource.ContainerName)
            .UseHostedService();
    });

builder.Services.AddEventStore(
    eventStoreBuilder =>
    {
        eventStoreBuilder
            .UseCosmosDb(eventStoreOptions =>
                {
                    eventStoreOptions.UseCosmosEmulator();
                    eventStoreOptions.EventStoreDatabaseId = cosmosOptions.DatabaseName;
                })
            .UseEvents(
                catalogBuilder =>
                {
                    catalogBuilder.FromAssembly<Program>();
                })
            .UseCQRS(
                cqrsBuilder =>
                {
                    cqrsBuilder.AddInitialization(1000);
                    cqrsBuilder.AddCommandsFromAssembly<Program>();
                    //// cqrsBuilder.AddProjectionJob<SomeProjection>(nameof(SomeProjection));
                });
    });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello World");

app.MapGet("/{amount}", (int amount, [FromQuery] string name) => $"Amount {amount} {name}");

//// app.MapGet("/name", ([FromQuery] string name, [FromServices] IMyHandler handler) => handler.Handle(name));

app.Run();

using System.Text.Json;
using System.Text.Json.Serialization;
using Atc.Cosmos;
using Microsoft.OpenApi.Models;
using Sample.ExpenseTracker.Endpoints;
using Sample.ExpenseTracker.Expenses.Projections;
using Sample.ExpenseTracker.Expenses.Views;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(
            c =>
            c.MapType<DateOnly>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "date",
            }));

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
        cosmosOptions.SerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false));

        builder.Services.ConfigureCosmos(
            cosmosOptions,
            cosmosBuilder
                => cosmosBuilder
                    .UseHostedService()
                    .AddContainer<ExpenseResourceInitializer, ExpenseResource>(ExpenseResource.ContainerName));

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
                            cqrsBuilder.AddProjectionJob<ExpenseProjection>(nameof(ExpenseProjection));
                        });
            });

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        ManagementEndpoints.MapEndpoints(app);
        ExpenseEndpoints.MapEndpoints(app);

        app.Run();
    }
}
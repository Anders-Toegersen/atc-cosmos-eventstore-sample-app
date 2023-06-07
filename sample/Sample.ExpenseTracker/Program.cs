using System.Text.Json;
using System.Text.Json.Serialization;
using Atc.Cosmos;
using Atc.Cosmos.EventStore.Cqrs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Sample.ExpenseTracker.Expenses.Commands;
using Sample.ExpenseTracker.Expenses.Models;
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
                Format = "date"
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

        builder.Services.ConfigureCosmos(
            cosmosOptions,
            cosmosBuilder
                => cosmosBuilder
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

        app.MapPost(
            "/register",
            async (
                [FromBody] UserRequest request,
                [FromServices] ICommandProcessor<RegisterUserCommand> processor,
                CancellationToken cancellationToken)
                =>
            {
                var commandResult = await processor.ExecuteAsync(
                    new RegisterUserCommand(
                        Guid.NewGuid(),
                        request.Username,
                        request.Email),
                    cancellationToken);

                return commandResult switch
                {
                    { Result: ResultType.Changed, Response: { } response } => Results.Json(response, statusCode: 201),
                    { Result: ResultType.Exists } => Results.Conflict("User already exists"),
                    _ => Results.Problem("Unexpected error occured")
                };
            });

        app.MapPost(
            "/{userId}/expense",
            async (
                [FromRoute] Guid userId,
                [FromBody] ExpenseRequest request,
                [FromServices] ICommandProcessor<RecordExpenseCommand> processor,
                CancellationToken cancellationToken)
                =>
            {
                var commandResult = await processor.ExecuteAsync(
                    new RecordExpenseCommand(
                        userId,
                        Guid.NewGuid(),
                        request.Description,
                        request.Amount,
                        request.Category,
                        request.Timestamp),
                    cancellationToken);

                return commandResult switch
                {
                    { Result: ResultType.Changed, Response: { } response } => Results.Json(response, statusCode: 201),
                    { Result: ResultType.NotFound } => Results.NotFound("User does not exist"),
                    _ => Results.Problem("Unexpected error occured")
                };
            });

        app.MapPut(
            "/{userId}/expense/{expenseId}",
            async (
                [FromRoute] Guid userId,
                [FromRoute] Guid expenseId,
                [FromBody] ExpenseRequest request,
                [FromServices] ICommandProcessor<UpdateExpenseCommand> processor,
                CancellationToken cancellationToken)
                =>
            {
                var commandResult = await processor.ExecuteAsync(
                    new UpdateExpenseCommand(
                        userId,
                        expenseId,
                        request.Description,
                        request.Amount,
                        request.Category,
                        request.Timestamp),
                    cancellationToken);

                return commandResult switch
                {
                    { Result: ResultType.Changed, Response: { } response } => Results.Json(response, statusCode: 200),
                    { Result: ResultType.NotModified, Response: { } response } => Results.NotFound(response),
                    _ => Results.Problem("Unexpected error occured")
                };
            });

        app.MapDelete(
            "/{userId}/expense/{expenseId}",
            async (
                [FromRoute] Guid userId,
                [FromRoute] Guid expenseId,
                [FromServices] ICommandProcessor<DeleteExpenseCommand> processor,
                CancellationToken cancellationToken)
                =>
            {
                var commandResult = await processor.ExecuteAsync(
                    new DeleteExpenseCommand(
                        userId,
                        expenseId),
                    cancellationToken);

                return commandResult switch
                {
                    { Result: ResultType.Changed, Response: { } response } => Results.Json(response, statusCode: 200),
                    { Result: ResultType.NotModified, Response: { } response } => Results.NotFound(response),
                    _ => Results.Problem("Unexpected error occured")
                };
            });

        app.Run();
    }
}
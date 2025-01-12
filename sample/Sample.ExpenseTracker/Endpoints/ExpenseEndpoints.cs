using Atc.Cosmos;
using Atc.Cosmos.EventStore.Cqrs;
using Microsoft.AspNetCore.Mvc;
using Sample.ExpenseTracker.Expenses.Commands;
using Sample.ExpenseTracker.Expenses.Models;
using Sample.ExpenseTracker.Expenses.Views;

namespace Sample.ExpenseTracker.Endpoints;

public static class ExpenseEndpoints
{
    public static void MapEndpoints(WebApplication app)
    {
        app.MapGet("/expenses/{expenseId}", DisplayExpense)
            .WithSummary(nameof(DisplayExpense))
            .WithTags("Expense")
            .WithOpenApi();

        app.MapPost("/expenses", CreateExpense)
            .WithSummary(nameof(CreateExpense))
            .WithTags("Expense")
            .WithOpenApi();

        app.MapPut("/expenses/{expenseId}", UpdateExpense)
            .WithSummary(nameof(UpdateExpense))
            .WithTags("Expense")
            .WithOpenApi();

        app.MapDelete("/expenses/{expenseId}", DeleteExpense)
            .WithSummary(nameof(DeleteExpense))
            .WithTags("Expense")
            .WithOpenApi();
    }

    private static async Task<IResult> DisplayExpense(
        [FromHeader] Guid userId,
        [FromRoute] Guid expenseId,
        [FromServices] ICosmosReader<ExpenseResource> reader,
        CancellationToken cancellationToken)
    {
        var queryResult = await reader.FindAsync(
            userId.ToString(),
            ExpenseResource.PartitionKey,
            cancellationToken);

        var expenseQuery = queryResult?.View.Expenses.SingleOrDefault(e => e.Id == expenseId.ToString());

        return expenseQuery switch
        {
            { } expense => Results.Json(expense, statusCode: 200),
            _ => Results.NotFound("User not found"),
        };
    }

    private static async Task<IResult> CreateExpense(
                [FromHeader] Guid userId,
                [FromBody] ExpenseRequest request,
                [FromServices] ICommandProcessor<RecordExpenseCommand> processor,
                CancellationToken cancellationToken)
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
            _ => Results.Problem("Unexpected error occured"),
        };
    }

    private static async Task<IResult> UpdateExpense(
        [FromHeader] Guid userId,
        [FromRoute] Guid expenseId,
        [FromBody] ExpenseRequest request,
        [FromServices] ICommandProcessor<UpdateExpenseCommand> processor,
        CancellationToken cancellationToken)
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
            _ => Results.Problem("Unexpected error occured"),
        };
    }

    private static async Task<IResult> DeleteExpense(
        [FromHeader] Guid userId,
        [FromRoute] Guid expenseId,
        [FromServices] ICommandProcessor<DeleteExpenseCommand> processor,
        CancellationToken cancellationToken)
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
            _ => Results.Problem("Unexpected error occured"),
        };
    }
}

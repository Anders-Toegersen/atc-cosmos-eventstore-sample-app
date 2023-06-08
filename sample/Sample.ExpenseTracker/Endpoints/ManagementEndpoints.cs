using Atc.Cosmos;
using Atc.Cosmos.EventStore.Cqrs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Sample.ExpenseTracker.Expenses.Commands;
using Sample.ExpenseTracker.Expenses.Models;
using Sample.ExpenseTracker.Expenses.Views;

namespace Sample.ExpenseTracker.Endpoints;

public static class ManagementEndpoints
{
    public static void MapEndpoints(WebApplication app)
    {
        app.MapPost("users/register", RegisterUser)
            .WithSummary(nameof(RegisterUser))
            .WithTags("Management")
            .WithOpenApi();

        app.MapGet("users/{userId}", DisplayUser)
            .WithSummary(nameof(DisplayUser))
            .WithTags("Management")
            .WithOpenApi();

        app.MapGet("/expenses/list/{status}", DisplayAllExpensesByStatus)
            .WithSummary(nameof(DisplayAllExpensesByStatus))
            .WithTags("Management")
            .WithOpenApi();

        app.MapPost("users/{userId}/expenses/{expenseId}/approve", ApproveExpenseForUserById)
            .WithSummary(nameof(ApproveExpenseForUserById))
            .WithTags("Management")
            .WithOpenApi();

        app.MapPost("users/{userId}/expenses/{expenseId}/reject", RejectExpenseForUserById)
            .WithSummary(nameof(RejectExpenseForUserById))
            .WithTags("Management")
            .WithOpenApi();
    }

    private static async Task<IResult> RegisterUser(
        [FromBody] UserRequest request,
        [FromServices] ICommandProcessor<RegisterUserCommand> processor,
        CancellationToken cancellationToken)
    {
        var commandResult = await processor.ExecuteAsync(
            new RegisterUserCommand(
                                Guid.NewGuid(),
                                request.Username,
                                request.Email),
                            cancellationToken);

        return commandResult switch
        {
            {
                Result: ResultType.Changed, Response: { } response
            } => Results.Json(response, statusCode: 201),
            { Result: ResultType.Exists } => Results.Conflict("User already exists"),
            _ => Results.Problem("Unexpected error occured"),
        };
    }

    private static async Task<IResult> DisplayUser(
        [FromHeader] Guid userId,
        [FromServices] ICosmosReader<ExpenseResource> reader,
        CancellationToken cancellationToken)
    {
        var queryResult = await reader.FindAsync(
            userId.ToString(),
            ExpenseResource.PartitionKey,
            cancellationToken);

        return queryResult switch
        {
            { View: { } view } => Results.Json(view, statusCode: 200),
            _ => Results.NotFound("User not found"),
        };
    }

    private static async Task<IResult> DisplayAllExpensesByStatus(
        [FromRoute] Status status,
        [FromServices] ICosmosReader<ExpenseResource> reader,
        CancellationToken cancellationToken)
    {
        var query = new QueryDefinition("""
                    SELECT c.id as userId, t AS expense
                    FROM c
                    JOIN t IN c.view.expenses
                    WHERE t.status = @Status
                    """)
        .WithParameter("@Status", status);

        var queryResult = reader.QueryAsync<UserExpense>(
            query,
            ExpenseResource.PartitionKey,
            cancellationToken);

        var expensesList = new List<UserExpense>();
        await foreach (var expense in queryResult.WithCancellation(cancellationToken))
        {
            expensesList.Add(expense);
        }

        return Results.Json(expensesList, statusCode: 200);
    }

    private static async Task<IResult> ApproveExpenseForUserById(
        [FromRoute] Guid userId,
        [FromRoute] Guid expenseId,
        [FromServices] ICommandProcessor<ApproveExpenseCommand> command,
        CancellationToken cancellationToken)
        => await command
            .ExecuteAsync(
                new ApproveExpenseCommand(
                    userId,
                    expenseId),
                cancellationToken)
            switch
        {
            { Result: ResultType.Changed } => Results.Ok(),
            { Result: ResultType.NotModified } => Results.NotFound(),
            _ => Results.Problem("Unexpected error occured"),
        };

    private static async Task<IResult> RejectExpenseForUserById(
        [FromRoute] Guid userId,
        [FromRoute] Guid expenseId,
        [FromBody] string? reason,
        [FromServices] ICommandProcessor<RejectExpenseCommand> command,
        CancellationToken cancellationToken)
        => await command
            .ExecuteAsync(
                new RejectExpenseCommand(
                    userId,
                    expenseId,
                    reason),
                cancellationToken)
            switch
        {
            { Result: ResultType.Changed } => Results.Ok(),
            { Result: ResultType.NotModified } => Results.NotFound(),
            _ => Results.Problem("Unexpected error occured"),
        };
}

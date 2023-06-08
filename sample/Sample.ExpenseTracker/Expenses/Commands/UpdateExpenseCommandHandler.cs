using Atc.Cosmos.EventStore.Cqrs;
using Sample.ExpenseTracker.Expenses.Events;
using Sample.ExpenseTracker.Expenses.Models;
using Sample.ExpenseTracker.Expenses.Projections;

namespace Sample.ExpenseTracker.Expenses.Commands;

public class UpdateExpenseCommandHandler :
    ExpenseReadModel,
    ICommandHandler<UpdateExpenseCommand>
{
    public ValueTask ExecuteAsync(
        UpdateExpenseCommand command,
        ICommandContext context,
        CancellationToken cancellationToken)
    {

        context.ResponseObject = base.GetExpenseById(command.ExpenseId) switch
        {
            { Status: Status.Approved } => "Cannot update an approved expense",
            { } => AddUpdatedEvent(context, command),
            _ => "Expense not found for User"
        };
        return ValueTask.CompletedTask;
    }

    private object AddUpdatedEvent(ICommandContext context, UpdateExpenseCommand command)
    {
        var evt = new ExpenseUpdatedEvent(
            command.ExpenseId,
            command.UserId,
            command.Description,
            command.Amount,
            command.Category,
            command.Timestamp);

        context.AddEvent(evt);
        Consume(evt);

        return base.View;
    }
}
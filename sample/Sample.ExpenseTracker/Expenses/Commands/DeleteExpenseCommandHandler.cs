using Atc.Cosmos.EventStore.Cqrs;
using Sample.ExpenseTracker.Expenses.Events;
using Sample.ExpenseTracker.Expenses.Models;
using Sample.ExpenseTracker.Expenses.Projections;

namespace Sample.ExpenseTracker.Expenses.Commands;

public class DeleteExpenseCommandHandler :
    ExpenseReadModel,
    ICommandHandler<DeleteExpenseCommand>
{
    public ValueTask ExecuteAsync(
        DeleteExpenseCommand command,
        ICommandContext context,
        CancellationToken cancellationToken)
    {
        context.ResponseObject = base.GetExpenseById(command.ExpenseId) switch
        {
            { Status: Status.Approved } => "Cannot delete an approved expense",
            { Status: Status.Rejected } => "Cannot delete a rejected expense",
            { } => AddDeletedEvent(context, command),
            _ => "Expense not found for User"
        };
        return ValueTask.CompletedTask;
    }

    private object AddDeletedEvent(ICommandContext context, DeleteExpenseCommand command)
    {
        var evt = new ExpenseDeletedEvent(
            command.ExpenseId,
            command.UserId);

        context.AddEvent(evt);
        base.Consume(evt);

        return base.View;
    }
}
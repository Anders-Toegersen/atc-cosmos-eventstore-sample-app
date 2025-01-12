using Atc.Cosmos.EventStore.Cqrs;
using Sample.ExpenseTracker.Expenses.Events;
using Sample.ExpenseTracker.Expenses.Models;
using Sample.ExpenseTracker.Expenses.Projections;

namespace Sample.ExpenseTracker.Expenses.Commands;

public class ApproveExpenseCommandHandler :
    ExpenseReadModel,
    ICommandHandler<ApproveExpenseCommand>
{
    public ValueTask ExecuteAsync(
        ApproveExpenseCommand command,
        ICommandContext context,
        CancellationToken cancellationToken)
    {
        if (base.GetExpenseById(command.ExpenseId)
            is { Status: Status.Submitted } or { Status: Status.Rejected })
        {
            var evt = new ExpenseApprovedEvent(
                command.ExpenseId,
                command.UserId);

            context.AddEvent(evt);
        }

        return ValueTask.CompletedTask;
    }
}
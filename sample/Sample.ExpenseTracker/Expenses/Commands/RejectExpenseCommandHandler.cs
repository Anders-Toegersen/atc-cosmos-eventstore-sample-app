using Atc.Cosmos.EventStore.Cqrs;
using Sample.ExpenseTracker.Expenses.Events;
using Sample.ExpenseTracker.Expenses.Projections;

namespace Sample.ExpenseTracker.Expenses.Commands;

public class RejectExpenseCommandHandler :
    ExpenseReadModel,
    ICommandHandler<RejectExpenseCommand>
{
    public ValueTask ExecuteAsync(
        RejectExpenseCommand command,
        ICommandContext context,
        CancellationToken cancellationToken)
    {
        if (base.GetExpenseById(command.ExpenseId) is not null)
        {
            var evt = new ExpenseRejectedEvent(
                command.ExpenseId,
                command.UserId,
                command.RejectionReason);

            context.AddEvent(evt);
        }

        return ValueTask.CompletedTask;
    }
}
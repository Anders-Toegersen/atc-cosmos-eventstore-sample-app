using Atc.Cosmos.EventStore.Cqrs;
using Sample.ExpenseTracker.Expenses.Events;
using Sample.ExpenseTracker.Expenses.Projections;

namespace Sample.ExpenseTracker.Expenses.Commands;

public class RecordExpenseCommandHandler :
    ExpenseReadModel,
    ICommandHandler<RecordExpenseCommand>
{
    public ValueTask ExecuteAsync(
        RecordExpenseCommand command,
        ICommandContext context,
        CancellationToken cancellationToken)
    {
        if (GetExpenseById(command.ExpenseId) is null)
        {
            var evt = new ExpenseRecordedEvent(
                command.ExpenseId,
                command.UserId,
                command.Description,
                command.Amount,
                command.Category,
                command.Timestamp);

            context.AddEvent(evt);
            Consume(evt);
        }

        context.ResponseObject = View;

        return ValueTask.CompletedTask;
    }
}
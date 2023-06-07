using Atc.Cosmos.EventStore.Cqrs;
using Sample.ExpenseTracker.Expenses.Events;
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
        if (GetExpenseById(command.ExpenseId) is not null)
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

            context.ResponseObject = View;

            return ValueTask.CompletedTask;
        }

        context.ResponseObject = "Expense not found for User";

        return ValueTask.CompletedTask;
    }
}
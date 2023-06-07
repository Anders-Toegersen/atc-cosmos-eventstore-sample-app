using Atc.Cosmos.EventStore.Cqrs;
using Clever.Firmware.Domain.FirmwareUpdates;
using Sample.ExpenseTracker.Expenses.Events;
using Sample.ExpenseTracker.Expenses.Projections;

namespace Sample.ExpenseTracker.Expenses.Commands;

public record DeleteExpenseCommand(
    Guid UserId,
    Guid ExpenseId)
    : CommandBase<UserStreamId>(
        new UserStreamId(UserId));

public class DeleteExpenseCommandHandler :
    ExpenseReadModel,
    ICommandHandler<DeleteExpenseCommand>
{
    public ValueTask ExecuteAsync(
        DeleteExpenseCommand command,
        ICommandContext context,
        CancellationToken cancellationToken)
    {
        if (GetExpenseById(command.ExpenseId) is not null)
        {
            var evt = new ExpenseDeletedEvent(
                command.ExpenseId,
                command.UserId);

            context.AddEvent(evt);
            Consume(evt);

            context.ResponseObject = View;

            return ValueTask.CompletedTask;
        }

        context.ResponseObject = "Expense not found for User";

        return ValueTask.CompletedTask;
    }
}
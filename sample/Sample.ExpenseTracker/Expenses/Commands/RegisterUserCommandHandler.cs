using Atc.Cosmos.EventStore.Cqrs;
using Sample.ExpenseTracker.Expenses.Events;
using Sample.ExpenseTracker.Expenses.Projections;

namespace Sample.ExpenseTracker.Expenses.Commands;

public class RegisterUserCommandHandler :
    ExpenseReadModel,
    ICommandHandler<RegisterUserCommand>
{
    public ValueTask ExecuteAsync(
        RegisterUserCommand command,
        ICommandContext context,
        CancellationToken cancellationToken)
    {
        var evt = new UserRegisteredEvent(
            command.UserId,
            command.Username,
            command.Email);

        context.AddEvent(evt);
        Consume(evt);

        context.ResponseObject = View;

        return ValueTask.CompletedTask;
    }
}
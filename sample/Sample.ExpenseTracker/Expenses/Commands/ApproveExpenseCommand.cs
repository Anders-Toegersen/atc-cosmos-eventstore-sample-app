using Atc.Cosmos.EventStore.Cqrs;

namespace Sample.ExpenseTracker.Expenses.Commands;

public record ApproveExpenseCommand(
    Guid UserId,
    Guid ExpenseId)
    : CommandBase<UserStreamId>(
        new UserStreamId(UserId));

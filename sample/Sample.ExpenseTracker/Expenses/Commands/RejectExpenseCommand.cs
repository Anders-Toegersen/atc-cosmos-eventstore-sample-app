using Atc.Cosmos.EventStore.Cqrs;

namespace Sample.ExpenseTracker.Expenses.Commands;

public record RejectExpenseCommand(
    Guid ExpenseId,
    Guid UserId,
    string? RejectionReason)
: CommandBase<UserStreamId>(
        new UserStreamId(UserId));
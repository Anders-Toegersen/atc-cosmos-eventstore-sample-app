using Atc.Cosmos.EventStore.Cqrs;
using Sample.ExpenseTracker.Expenses.Models;

namespace Sample.ExpenseTracker.Expenses.Commands;

public record RejectExpenseCommand(
    Guid ExpenseId,
    Guid UserId,
    Status Status,
    string? RejectionReason,
    Category Category)
: CommandBase<UserStreamId>(
        new UserStreamId(UserId));
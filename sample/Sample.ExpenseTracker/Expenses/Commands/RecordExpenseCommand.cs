using Atc.Cosmos.EventStore.Cqrs;
using Sample.ExpenseTracker.Expenses.Models;

namespace Sample.ExpenseTracker.Expenses.Commands;

public record RecordExpenseCommand(
    Guid UserId,
    Guid ExpenseId,
    string Description,
    decimal Amount,
    Category Category,
    DateOnly Timestamp)
: CommandBase<UserStreamId>(
        new UserStreamId(UserId),
        RequiredVersion: EventStreamVersion.Exists);

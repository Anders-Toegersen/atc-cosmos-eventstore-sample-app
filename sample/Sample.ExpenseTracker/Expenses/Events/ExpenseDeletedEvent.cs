using Atc.Cosmos.EventStore.Cqrs;

namespace Sample.ExpenseTracker.Expenses.Events;

[StreamEvent("expense-deleted:v1")]
public record ExpenseDeletedEvent(
    Guid ExpenseId,
    Guid UserId);

using Atc.Cosmos.EventStore.Cqrs;

namespace Sample.ExpenseTracker.Events;

[StreamEvent("expense-deleted:v1")]
public record ExpenseDeletedEvent(
    Guid ExpenseId,
    Guid UserId);

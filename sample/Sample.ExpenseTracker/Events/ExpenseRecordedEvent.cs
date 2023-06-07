using Atc.Cosmos.EventStore.Cqrs;

namespace Sample.ExpenseTracker.Events;

[StreamEvent("expense-recorded:v1")]
public record ExpenseRecordedEvent(
    Guid ExpenseId,
    Guid UserId,
    decimal Amount,
    string Category,
    DateTime Timestamp);

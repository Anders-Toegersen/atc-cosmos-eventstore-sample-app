using Atc.Cosmos.EventStore.Cqrs;

namespace Sample.ExpenseTracker.Events;

[StreamEvent("expense-updated:v1")]
public record ExpenseUpdatedEvent(
    Guid ExpenseId,
    Guid UserId,
    decimal Amount,
    string Category);

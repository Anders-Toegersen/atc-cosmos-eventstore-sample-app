using Atc.Cosmos.EventStore.Cqrs;

namespace Sample.ExpenseTracker.Events;

[StreamEvent("expense-categorized:v1")]
public record ExpenseCategorizedEvent(
    Guid ExpenseId,
    Guid UserId,
    string Category);

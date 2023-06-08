using Atc.Cosmos.EventStore.Cqrs;

namespace Sample.ExpenseTracker.Expenses.Events;

[StreamEvent("expense-rejected:v1")]
public record ExpenseRejectedEvent(
    Guid ExpenseId,
    Guid UserId,
    string? RejectionReason);

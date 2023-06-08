using Atc.Cosmos.EventStore.Cqrs;

namespace Sample.ExpenseTracker.Expenses.Events;

[StreamEvent("expense-approved:v1")]
public record ExpenseApprovedEvent(
    Guid ExpenseId,
    Guid UserId);

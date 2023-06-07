using Atc.Cosmos.EventStore.Cqrs;
using Sample.ExpenseTracker.Expenses.Models;

namespace Sample.ExpenseTracker.Expenses.Events;

[StreamEvent("expense-updated:v1")]
public record ExpenseUpdatedEvent(
    Guid ExpenseId,
    Guid UserId,
    string Description,
    decimal Amount,
    Category Category,
    DateOnly Timestamp);

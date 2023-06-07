using Atc.Cosmos.EventStore.Cqrs;
using Sample.ExpenseTracker.Expenses.Models;

namespace Sample.ExpenseTracker.Expenses.Events;

[StreamEvent("expense-recorded:v1")]
public record ExpenseRecordedEvent(
    Guid ExpenseId,
    Guid UserId,
    string Description,
    decimal Amount,
    Category Category,
    DateOnly Timestamp);

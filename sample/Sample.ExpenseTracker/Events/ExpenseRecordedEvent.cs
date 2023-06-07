using Atc.Cosmos.EventStore.Cqrs;
using Sample.ExpenseTracker.Models;

namespace Sample.ExpenseTracker.Events;

[StreamEvent("expense-recorded:v1")]
public record ExpenseRecordedEvent(
    Guid ExpenseId,
    Guid UserId,
    decimal Amount,
    Category Category,
    DateTime Timestamp);

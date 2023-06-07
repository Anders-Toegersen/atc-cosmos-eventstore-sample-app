using Atc.Cosmos.EventStore.Cqrs;
using Sample.ExpenseTracker.Models;

namespace Sample.ExpenseTracker.Events;

[StreamEvent("expense-updated:v1")]
public record ExpenseUpdatedEvent(
    Guid ExpenseId,
    Guid UserId,
    decimal Amount,
    Category Category);

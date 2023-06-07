using Atc.Cosmos.EventStore.Cqrs;
using Sample.ExpenseTracker.Models;

namespace Sample.ExpenseTracker.Events;

[StreamEvent("expense-approved:v1")]
public record ExpenseApprovedEvent(
    Guid ExpenseId,
    Guid UserId,
    Status Status,
    Category Category);

using Atc.Cosmos.EventStore.Cqrs;
using Sample.ExpenseTracker.Models;

namespace Sample.ExpenseTracker.Events;

[StreamEvent("expense-rejected:v1")]
public record ExpenseRejectedEvent(
    Guid ExpenseId,
    Guid UserId,
    Status Status,
    string? RejectionReason,
    Category Category);

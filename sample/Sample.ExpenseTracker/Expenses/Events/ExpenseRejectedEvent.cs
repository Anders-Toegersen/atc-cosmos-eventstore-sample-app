using Atc.Cosmos.EventStore.Cqrs;
using Sample.ExpenseTracker.Expenses.Models;

namespace Sample.ExpenseTracker.Expenses.Events;

[StreamEvent("expense-rejected:v1")]
public record ExpenseRejectedEvent(
    Guid ExpenseId,
    Guid UserId,
    Status Status,
    string? RejectionReason,
    Category Category);

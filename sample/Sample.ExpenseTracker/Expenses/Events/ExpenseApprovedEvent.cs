using Atc.Cosmos.EventStore.Cqrs;
using Sample.ExpenseTracker.Expenses.Models;

namespace Sample.ExpenseTracker.Expenses.Events;

[StreamEvent("expense-approved:v1")]
public record ExpenseApprovedEvent(
    Guid ExpenseId,
    Guid UserId,
    Category Category);

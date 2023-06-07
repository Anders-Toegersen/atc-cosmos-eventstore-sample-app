using Atc.Cosmos.EventStore.Cqrs;
using Sample.ExpenseTracker.Models;

namespace Sample.ExpenseTracker.Events;

[StreamEvent("expense-categorized:v1")]
public record ExpenseCategorizedEvent(
    Guid ExpenseId,
    Guid UserId,
    Category Category);

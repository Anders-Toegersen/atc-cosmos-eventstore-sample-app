using Atc.Cosmos.EventStore.Cqrs;
using Sample.ExpenseTracker.Expenses.Models;

namespace Sample.ExpenseTracker.Expenses.Events;

[StreamEvent("expense-categorized:v1")]
public record ExpenseCategorizedEvent(
    Guid ExpenseId,
    Guid UserId,
    Category Category);

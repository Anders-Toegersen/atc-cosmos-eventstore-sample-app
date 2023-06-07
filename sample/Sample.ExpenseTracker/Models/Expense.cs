namespace Sample.ExpenseTracker.Models;

public record Expense(
    string Id,
    string Description,
    Status Status,
    decimal Amount,
    DateTimeOffset Timestamp
);

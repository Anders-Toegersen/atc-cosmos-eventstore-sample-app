namespace Sample.ExpenseTracker.Models;

public record Expense(
    string Id,
    string Description,
    Category Category,
    Status Status,
    decimal Amount,
    string? RejectionReason,
    DateTimeOffset Timestamp
);

namespace Sample.ExpenseTracker.Expenses.Models;

public class Expense
{
    public required string Id { get; set; }

    public string Description { get; set; } = default!;

    public Category Category { get; set; }

    public Status Status { get; set; }

    public decimal Amount { get; set; }

    public string? RejectionReason { get; set; }

    public DateOnly Timestamp { get; set; }
}

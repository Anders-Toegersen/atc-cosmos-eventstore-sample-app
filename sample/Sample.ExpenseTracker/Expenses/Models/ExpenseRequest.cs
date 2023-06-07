namespace Sample.ExpenseTracker.Expenses.Models;

public class ExpenseRequest
{
    public string Description { get; set; } = default!;

    public Category Category { get; set; }

    public decimal Amount { get; set; }

    public DateOnly Timestamp { get; set; }
}

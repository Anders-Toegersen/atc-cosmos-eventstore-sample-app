using Sample.ExpenseTracker.Models;

namespace Sample.ExpenseTracker.Views;

public class ExpenseView
{
    public required string UserId { get; set; } = default!;

    public string Email { get; set; } = default!;

    public DateTimeOffset Timestamp { get; set; }

    public List<Expense> Expenses { get; set; } = new();

}
using Sample.ExpenseTracker.Expenses.Models;

namespace Sample.ExpenseTracker.Expenses.Views;

public class ExpenseView
{
    public required Guid UserId { get; set; }

    public string UserName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public DateTimeOffset Timestamp { get; set; }

    public List<Expense> Expenses { get; set; } = new();

}
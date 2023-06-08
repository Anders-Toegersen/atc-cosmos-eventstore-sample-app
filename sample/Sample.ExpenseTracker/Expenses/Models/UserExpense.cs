namespace Sample.ExpenseTracker.Expenses.Models;

public class UserExpense
{
    public Guid UserId { get; set; }

    public Expense Expense { get; set; } = default!;
}

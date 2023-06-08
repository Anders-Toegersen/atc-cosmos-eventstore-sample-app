using Atc.Cosmos.EventStore.Cqrs;
using Sample.ExpenseTracker.Expenses.Events;
using Sample.ExpenseTracker.Expenses.Models;
using Sample.ExpenseTracker.Expenses.Views;

namespace Sample.ExpenseTracker.Expenses.Projections;

public class ExpenseReadModel :
    IConsumeEvent<ExpenseApprovedEvent>,
    IConsumeEvent<ExpenseRejectedEvent>,
    IConsumeEvent<UserRegisteredEvent>,
    IConsumeEvent<ExpenseRecordedEvent>,
    IConsumeEvent<ExpenseDeletedEvent>,
    IConsumeEvent<ExpenseUpdatedEvent>
{
    public ExpenseResource Resource { get; set; } = new() { View = new ExpenseView { UserId = Guid.Empty } };

    public ExpenseView View => Resource.View;

    public void Consume(
        ExpenseApprovedEvent evt,
        EventMetadata metadata = default!)
    {
        var expense = GetExpenseById(evt.ExpenseId);
        if (expense is not null)
        {
            expense.Status = Status.Approved;
        }
    }

    public void Consume(
        ExpenseRejectedEvent evt,
        EventMetadata metadata)
    {
        var expense = GetExpenseById(evt.ExpenseId);
        if (expense is not null)
        {
            expense.Status = Status.Rejected;
        }
    }

    public void Consume(
        UserRegisteredEvent evt,
        EventMetadata metadata = default!)
    {
        View.UserId = evt.UserId;
        View.UserName = evt.Username;
        View.Email = evt.Email;
    }

    public void Consume(
        ExpenseRecordedEvent evt,
        EventMetadata metadata = default!)
        => View.Expenses.Add(
            new Expense
            {
                Id = evt.ExpenseId.ToString(),
                Amount = evt.Amount,
                Description = evt.Description,
                Status = Status.Submitted,
                Category = evt.Category,
                Timestamp = evt.Timestamp,
            });

    public void Consume(
        ExpenseUpdatedEvent evt,
        EventMetadata metadata = default!)
    {
        var expense = GetExpenseById(evt.ExpenseId);
        if (expense is not null)
        {
            expense.Amount = evt.Amount;
            expense.Description = evt.Description;
            expense.Status = Status.Submitted;
            expense.Category = evt.Category;
            expense.Timestamp = evt.Timestamp;
        }
    }

    public void Consume(
        ExpenseDeletedEvent evt,
        EventMetadata metadata = default!)
        => View.Expenses.RemoveAll(e => e.Id == evt.ExpenseId.ToString());

    protected Expense? GetExpenseById(Guid id)
        => View.Expenses.SingleOrDefault(e => e.Id == id.ToString());
}

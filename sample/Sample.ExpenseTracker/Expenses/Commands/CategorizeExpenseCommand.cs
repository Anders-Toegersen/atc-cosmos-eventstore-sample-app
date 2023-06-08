using Atc.Cosmos.EventStore.Cqrs;
using Sample.ExpenseTracker.Expenses.Models;

namespace Sample.ExpenseTracker.Expenses.Commands;

public record CategorizeExpenseCommand(
    Guid userId,
    Guid ExpenseId,
    Category Category)
    : CommandBase<UserStreamId>(
        new UserStreamId(userId));
using Atc.Cosmos.EventStore.Cqrs;

namespace Sample.ExpenseTracker.Expenses.Events;

[StreamEvent("user-registered:v1")]
public record UserRegisteredEvent(
    Guid UserId,
    string Username,
    string Email);

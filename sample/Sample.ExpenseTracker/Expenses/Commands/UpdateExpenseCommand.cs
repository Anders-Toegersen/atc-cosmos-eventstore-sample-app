using Atc.Cosmos.EventStore.Cqrs;
using Clever.Firmware.Domain.FirmwareUpdates;
using Sample.ExpenseTracker.Expenses.Models;

namespace Sample.ExpenseTracker.Expenses.Commands;

public record UpdateExpenseCommand(
    Guid UserId,
    Guid ExpenseId,
    string Description,
    decimal Amount,
    Category Category,
    DateOnly Timestamp)
: CommandBase<UserStreamId>(
        new UserStreamId(UserId));

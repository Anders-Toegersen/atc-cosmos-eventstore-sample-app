using Atc.Cosmos.EventStore.Cqrs;
using Clever.Firmware.Domain.FirmwareUpdates;
using Sample.ExpenseTracker.Expenses.Models;

namespace Sample.ExpenseTracker.Expenses.Events;

public record RejectExpenseCommand(
    Guid ExpenseId,
    Guid UserId,
    Status Status,
    string? RejectionReason,
    Category Category)
: CommandBase<UserStreamId>(
        new UserStreamId(UserId));
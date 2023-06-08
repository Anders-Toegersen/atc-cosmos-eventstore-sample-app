using Atc.Cosmos.EventStore.Cqrs;
using Clever.Firmware.Domain.FirmwareUpdates;

namespace Sample.ExpenseTracker.Expenses.Commands;

public record DeleteExpenseCommand(
    Guid UserId,
    Guid ExpenseId)
    : CommandBase<UserStreamId>(
        new UserStreamId(UserId));

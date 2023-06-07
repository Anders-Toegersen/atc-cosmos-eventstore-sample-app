using Atc.Cosmos.EventStore.Cqrs;
using Clever.Firmware.Domain.FirmwareUpdates;
using Sample.ExpenseTracker.Expenses.Models;

namespace Sample.ExpenseTracker.Expenses.Events;

public record ApproveExpenseCommand(
    Guid userId,
    Guid ExpenseId,
    Category Category)
    : CommandBase<UserStreamId>(
        new UserStreamId(userId));

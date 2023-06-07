using Atc.Cosmos.EventStore.Cqrs;
using Clever.Firmware.Domain.FirmwareUpdates;

namespace Sample.ExpenseTracker.Expenses.Commands;

public record RegisterUserCommand(
    Guid UserId,
    string Username,
    string Email)
: CommandBase<UserStreamId>(
    new UserStreamId(UserId),
    RequiredVersion: EventStreamVersion.StreamEmpty);

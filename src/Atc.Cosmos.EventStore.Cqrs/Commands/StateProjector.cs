using Atc.Cosmos.EventStore.Streams;

namespace Atc.Cosmos.EventStore.Cqrs.Commands;

internal class StateProjector<TCommand> : IStateProjector<TCommand>
    where TCommand : ICommand
{
    private readonly IEventStoreClient eventStore;
    private readonly IStreamReadValidator readValidator;
    private readonly ICommandHandlerMetadata<TCommand> handlerMetadata;

    public StateProjector(
        IEventStoreClient eventStore,
        IStreamReadValidator readValidator,
        ICommandHandlerMetadata<TCommand> handlerMetadata)
    {
        this.eventStore = eventStore;
        this.readValidator = readValidator;
        this.handlerMetadata = handlerMetadata;
    }

    public async ValueTask<IStreamState> ProjectAsync(
        TCommand command,
        ICommandHandler<TCommand> handler,
        CancellationToken cancellationToken)
    {
        var state = new StreamState
        {
            Id = command.GetEventStreamId().Value,
            Version = 0L,
        };

        if (handlerMetadata.IsNotConsumingEvents())
        {
            var metadata = await eventStore
                .GetStreamInfoAsync(
                    state.Id,
                    cancellationToken)
                .ConfigureAwait(false);

            readValidator.Validate(
                metadata,
                command.RequiredVersion?.Value ?? StreamVersion.Any);

            state.Version = metadata.Version;

            return state;
        }

        await foreach (var evt in eventStore
            .ReadFromStreamAsync(
                state.Id,
                command.RequiredVersion?.Value ?? StreamVersion.Any,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false))
        {
            await handlerMetadata
                .ConsumeAsync(
                    evt,
                    handler,
                    cancellationToken)
                .ConfigureAwait(false);

            state.Version = evt.Metadata.Version;
        }

        return state;
    }
}
using Atc.Cosmos;
using Atc.Cosmos.EventStore.Cqrs;
using Sample.ExpenseTracker.Expenses.Views;

namespace Sample.ExpenseTracker.Expenses.Projections;

[ProjectionFilter(UserStreamId.FilterIncludeAllEvents)]
public class ExpenseProjection :
    ExpenseReadModel,
    IProjection
{
    private readonly ICosmosReader<ExpenseResource> reader;
    private readonly ICosmosWriter<ExpenseResource> writer;

    public ExpenseProjection(
        ICosmosReader<ExpenseResource> reader,
        ICosmosWriter<ExpenseResource> writer)
    {
        this.reader = reader;
        this.writer = writer;
    }

    public Task CompleteAsync(
        CancellationToken cancellationToken)
        => writer.WriteAsync(Resource, cancellationToken);

    public Task<ProjectionAction> FailedAsync(Exception exception, CancellationToken cancellationToken)
    => Task.FromResult(ProjectionAction.Continue);

    public async Task InitializeAsync(
        EventStreamId id,
        CancellationToken cancellationToken)
    {
        var userId = new UserStreamId(id);
        Resource = await reader.FindAsync(userId.Id.ToString(), ExpenseResource.PartitionKey, cancellationToken)
            ?? new ExpenseResource
            {
                View = new ExpenseView
                {
                    UserId = userId.Id,

                },
            };
    }
}

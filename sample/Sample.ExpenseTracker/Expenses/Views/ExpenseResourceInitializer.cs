using Atc.Cosmos;
using Microsoft.Azure.Cosmos;

namespace Sample.ExpenseTracker.Expenses.Views;

public class ExpenseResourceInitializer : ICosmosContainerInitializer
{
    public Task InitializeAsync(
        Database database,
        CancellationToken cancellationToken)
        => database.CreateContainerIfNotExistsAsync(
            new ContainerProperties(
                ExpenseResource.ContainerName,
                "/pk"),
            ThroughputProperties.CreateManualThroughput(1000),
            cancellationToken: cancellationToken);
}
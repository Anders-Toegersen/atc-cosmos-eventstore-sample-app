using Atc.Cosmos;

namespace Sample.ExpenseTracker.Storage;

public class SomeResource : CosmosResource
{
    public const string PartitonKey = "my-partition-key";

    public const string ContainerName = "SomeContainerName";

    public string Id => View.Id;

    public string Pk => PartitonKey;

    public required SomeView View { get; set; }

    protected override string GetDocumentId() => Id;

    protected override string GetPartitionKey() => Pk;
}

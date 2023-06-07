using Atc.Cosmos;

namespace Sample.ExpenseTracker.Expenses.Views;

public class ExpenseResource : CosmosResource
{
    public const string PartitionKey = "expense";

    public const string ContainerName = "expenses";

    public string Id => View.UserId.ToString();

    public string Pk => PartitionKey;

    public required ExpenseView View { get; set; }

    protected override string GetDocumentId() => Id;

    protected override string GetPartitionKey() => Pk;
}

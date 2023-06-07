using System.Text.Json.Serialization;
using Atc.Cosmos;

namespace Sample.ExpenseTracker.Views;

public class ExpenseResource : CosmosResource
{
    [JsonPropertyName("id")]
    public string Id => View.UserId;

    [JsonPropertyName("pk")]
    public string PartitionKey { get; set; } = default!;

    public required ExpenseView View { get; set; }

    protected override string GetDocumentId()
        => Id;

    protected override string GetPartitionKey()
        => PartitionKey;
}

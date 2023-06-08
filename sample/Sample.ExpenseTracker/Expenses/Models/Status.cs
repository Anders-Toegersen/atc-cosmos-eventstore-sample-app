using System.Text.Json.Serialization;

namespace Sample.ExpenseTracker.Expenses.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Status
{
    Submitted,
    Approved,
    Rejected,
}
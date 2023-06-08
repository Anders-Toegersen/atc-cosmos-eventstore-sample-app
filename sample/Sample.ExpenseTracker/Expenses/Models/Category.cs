using System.Text.Json.Serialization;

namespace Sample.ExpenseTracker.Expenses.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Category
{
    Food,
    Travel,
    Accommodation,
    Other,
}
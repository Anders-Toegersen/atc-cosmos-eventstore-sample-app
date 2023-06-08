using Atc.Cosmos.EventStore.Cqrs;

namespace Sample.ExpenseTracker.Expenses;

public sealed class UserStreamId : EventStreamId, IEquatable<UserStreamId?>
{
    public const string TypeName = "expense";
    public const string FilterIncludeAllEvents = TypeName + ".*";

    public UserStreamId(EventStreamId id)
        : base(id.Parts.ToArray())
        => Id = new(id.Parts[1]);

    public UserStreamId(Guid userId)
        : base(TypeName, userId.ToString())
        => Id = userId;

    public Guid Id { get; }

    public override bool Equals(object? obj)
        => Equals(obj as UserStreamId);

    public bool Equals(UserStreamId? other)
        => other != null && Value == other.Value;

    public override int GetHashCode()
        => HashCode.Combine(Value);
}

namespace Atc.Cosmos.EventStore;

public class ConsumerGroup
{
    private const string DefaultInstance = ".default";

    public ConsumerGroup(string name)
        : this(name, DefaultInstance)
    {
    }

    public ConsumerGroup(string name, string instance)
    {
        Name = name;
        Instance = instance;
    }

    public string Name { get; }

    public string Instance { get; }

    public static ConsumerGroup GetAsAutoScalingInstance(string name)
        => new(name, Guid.NewGuid().ToString());
}
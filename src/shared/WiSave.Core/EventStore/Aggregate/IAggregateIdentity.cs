namespace WiSave.Core.EventStore.Aggregate;

public interface IAggregateIdentity
{
    Guid Value { get; init; }
}
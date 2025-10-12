namespace WiSave.Core.EventStore.Aggregate;

public interface IAggregateRepository<TAggregate, in TIdentity> where TAggregate : Aggregate<TIdentity>, new() where TIdentity : class, IAggregateIdentity
{
    Task<TAggregate?> GetById(TIdentity id, CancellationToken cancellationToken = default);
    Task Save(TAggregate aggregate, CancellationToken cancellationToken = default);
}
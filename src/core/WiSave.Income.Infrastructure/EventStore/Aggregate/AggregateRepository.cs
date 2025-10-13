using KurrentDB.Client;
using MassTransit;
using WiSave.Core.EventStore.Aggregate;
using WiSave.Core.EventStore.Mappers;
using WiSave.Core.EventStore.Serialization;

namespace WiSave.Income.Infrastructure.EventStore.Aggregate;

public class EventStoreAggregateRepository<TAggregate, TIdentity>(KurrentDBClient client, IPublishEndpoint publishEndpoint) : IAggregateRepository<TAggregate, TIdentity> where TAggregate : Aggregate<TIdentity>, new() where TIdentity : class, IAggregateIdentity
{
    public async Task<TAggregate?> GetById(TIdentity id, CancellationToken cancellationToken = default)
    {
        var streamName = StreamNameMapper.ToStreamId<TAggregate>(id.Value);

        var readResult = client.ReadStreamAsync(
            Direction.Forwards,
            streamName,
            StreamPosition.Start,
            cancellationToken: cancellationToken
        );

        var state = await readResult.ReadState;
        if (state == ReadState.StreamNotFound)
            return null;

        var events = await readResult
            .Select(resolvedEvent => resolvedEvent.Deserialize())
            .Where(evt => evt != null)
            .ToListAsync(cancellationToken: cancellationToken);

        if (events.Count == 0)
            return null;

        var aggregate = new TAggregate();
        aggregate.LoadFromHistory(events!);

        return aggregate;
    }

    public async Task Save(TAggregate aggregate, CancellationToken ct = default)
    {
        var streamName = StreamNameMapper.ToStreamId<TAggregate>(aggregate.Identifier.Value);
        var uncommittedEvents = aggregate.GetUncommittedEvents().ToList();
    
        if (uncommittedEvents.Count == 0)
            return;
    
        var eventData = uncommittedEvents.Select(evt => evt.ToJsonEventData());
        
        await client.AppendToStreamAsync(
            streamName,
            aggregate.Version,
            eventData,
            cancellationToken: ct
        );
        
        foreach (var @event in uncommittedEvents)
        {
            await publishEndpoint.Publish(@event, ct);
        }
        
        aggregate.MarkEventsAsCommitted();
    }
}
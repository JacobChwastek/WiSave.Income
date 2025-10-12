using System.Reflection;

namespace WiSave.Core.EventStore.Aggregate;

public abstract class Aggregate<TIdentity> where TIdentity : class, IAggregateIdentity
{
    private readonly List<object> _uncommittedEvents = [];
    
    public TIdentity Identifier { get; protected set; } = null!;
    public ulong Version { get; private set; }

    protected void RaiseEvent(object @event)
    {
        Play(@event);
        _uncommittedEvents.Add(@event);
        Version++;
    }

    public void LoadFromHistory(IEnumerable<object> events)
    {
        foreach (var @event in events)
        {
            Play(@event);
            Version++;
        }
    }

    public IEnumerable<object> GetUncommittedEvents() => _uncommittedEvents.AsReadOnly();

    public void MarkEventsAsCommitted() => _uncommittedEvents.Clear();

    private void Play(object @event)
    {
        var eventType = @event.GetType();
        var aggregateType = GetType();
        var playMethod = aggregateType.GetMethod(
            "Play", 
            BindingFlags.Instance | BindingFlags.NonPublic,
            null, 
            [eventType], 
            null);
        
        if (playMethod == null)
        {
            throw new InvalidOperationException(
                $"Aggregate '{aggregateType.Name}' does not implement Play({eventType.Name})");
        }

        playMethod.Invoke(this, [@event]);
    }
}
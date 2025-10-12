using System.Text.Json;
using KurrentDB.Client;
using OpenTelemetry.Context.Propagation;

namespace WiSave.Core.EventStore.Serialization;

public static class EventStoreDbSerializer
{
    private static readonly JsonSerializerOptions SerializerOptions;

    static EventStoreDbSerializer()
    {
        SerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public static T? Deserialize<T>(this ResolvedEvent resolvedEvent) where T : class => Deserialize(resolvedEvent) as T;

    public static object? Deserialize(this ResolvedEvent resolvedEvent)
    {
        var eventType = EventTypeMapper.Instance.ToType(resolvedEvent.Event.EventType);

        if (eventType == null)
            return null;

        return JsonSerializer.Deserialize(
            resolvedEvent.Event.Data.Span,
            eventType,
            SerializerOptions
        );
    }

    public static PropagationContext? DeserializePropagationContext(this ResolvedEvent resolvedEvent)
    {
        var eventType = EventTypeMapper.Instance.ToType(resolvedEvent.Event.EventType);

        if (eventType == null)
            return null;

        return JsonSerializer.Deserialize<PropagationContext>(
            resolvedEvent.Event.Metadata.Span,
            SerializerOptions
        );
    }

    public static EventData ToJsonEventData(this object @event, object? metadata = null) =>
        new(
            Uuid.NewUuid(),
            EventTypeMapper.Instance.ToName(@event.GetType()),
            JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), SerializerOptions),
            JsonSerializer.SerializeToUtf8Bytes(metadata ?? new { }, SerializerOptions)
        );
}
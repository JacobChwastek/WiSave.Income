namespace WiSave.Shared.Telemetry;

/// <summary>
/// Standardized tag names for WiSave telemetry
/// These are namespace-agnostic and work with any ActivitySource
/// </summary>
public static class WiSaveTelemetryTags
{
    public static class Commands
    {
        public const string Command = "wisave.command";
        public const string CommandType = "wisave.command.type";
        public const string CommandId = "wisave.command.id";
        public const string CommandUser = "wisave.command.user";
    }

    public static class Events
    {
        public const string Event = "wisave.event";
        public const string EventType = "wisave.event.type";
        public const string EventId = "wisave.event.id";
        public const string EventTimestamp = "wisave.event.timestamp";
        public const string EventStreamId = "wisave.event.stream_id";
    }

    public static class Entities
    {
        public const string EntityType = "wisave.entity.type";
        public const string EntityId = "wisave.entity.id";
        public const string EntityVersion = "wisave.entity.version";
    }

    public static class Messaging
    {
        public const string System = "messaging.system";
        public const string Operation = "messaging.operation";
        public const string Destination = "messaging.destination";
        public const string MessageId = "messaging.message_id";
        public const string ConversationId = "messaging.conversation_id";
    }
    
    public static class Http
    {
        public const string Method = "http.method";
        public const string StatusCode = "http.status_code";
        public const string Url = "http.url";
        public const string Route = "http.route";
    }
    
    public static class Database
    {
        public const string System = "db.system";
        public const string Statement = "db.statement";
        public const string Operation = "db.operation";
    }
}
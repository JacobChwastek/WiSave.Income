using System.Diagnostics;

namespace WiSave.Shared.Telemetry;

public class StartActivityOptions
{
    public Dictionary<string, object?> Tags { get; set; } = new();
    public string? ParentId { get; set; }
    public ActivityContext? Parent { get; set; }
    public ActivityKind Kind { get; set; } = ActivityKind.Internal;
}
using System.Diagnostics;

namespace WiSave.Shared.Telemetry;

/// <summary>
/// Provides access to the service's ActivitySource
/// Now injected per-service instead of static
/// </summary>
public interface IActivitySourceProvider
{
    ActivitySource Source { get; }
    string SourceName { get; }
}

public class ActivitySourceProvider(string sourceName, string? version = null) : IActivitySourceProvider
{
    public ActivitySource Source { get; } = new(sourceName, version ?? "1.0.0");
    public string SourceName { get; } = sourceName;
}

/// <summary>
/// Static helper for backward compatibility or simple scenarios
/// </summary>
public static class DefaultActivitySource
{
    private const string WiSaveNamespace = "wisave";
    
    public static string GetSourceName(string serviceType, string serviceName)
    {
        return $"{WiSaveNamespace}.{serviceType}.{serviceName}";
    }
}
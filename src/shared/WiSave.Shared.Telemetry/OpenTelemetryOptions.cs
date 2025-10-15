using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace WiSave.Shared.Telemetry;

public class OpenTelemetryOptions
{
    public static OpenTelemetryOptions Default => new();

    public string ServiceVersion { get; set; } = "1.0.0";
    public Func<TracerProviderBuilder, TracerProviderBuilder> ConfigureTracerProvider { get; private set; } = p => p;
    public Func<MeterProviderBuilder, MeterProviderBuilder> ConfigureMeterProvider { get; private set; } = p => p;
    public bool ShouldDisableConsoleExporter { get; private set; }

    private OpenTelemetryOptions() { }

    public static OpenTelemetryOptions Build(Action<OpenTelemetryOptions> configure)
    {
        var options = Default;
        configure(options);
        return options;
    }

    public OpenTelemetryOptions WithTracing(Func<TracerProviderBuilder, TracerProviderBuilder> configure)
    {
        var previousConfig = ConfigureTracerProvider;
        ConfigureTracerProvider = builder => configure(previousConfig(builder));
        return this;
    }

    public OpenTelemetryOptions WithMetrics(Func<MeterProviderBuilder, MeterProviderBuilder> configure)
    {
        var previousConfig = ConfigureMeterProvider;
        ConfigureMeterProvider = builder => configure(previousConfig(builder));
        return this;
    }

    public OpenTelemetryOptions WithServiceVersion(string version)
    {
        ServiceVersion = version;
        return this;
    }

    public OpenTelemetryOptions DisableConsoleExporter(bool shouldDisable = true)
    {
        ShouldDisableConsoleExporter = shouldDisable;
        return this;
    }
    
    /// <summary>
    /// Adds MongoDB instrumentation
    /// </summary>
    public OpenTelemetryOptions WithMongoDbInstrumentation()
    {
        return WithTracing(builder =>
        {
            builder.AddSource("MongoDB.Driver.Core.Extensions.DiagnosticSources");
            return builder;
        });
    }
    
    /// <summary>
    /// Adds custom ActivitySource
    /// </summary>
    public OpenTelemetryOptions WithSource(string sourceName)
    {
        return WithTracing(builder =>
        {
            builder.AddSource(sourceName);
            return builder;
        });
    }

    /// <summary>
    /// Adds custom Meter
    /// </summary>
    public OpenTelemetryOptions WithMeter(string meterName)
    {
        return WithMetrics(builder =>
        {
            builder.AddMeter(meterName);
            return builder;
        });
    }
}
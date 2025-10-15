using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using WiSave.Shared.Telemetry;

namespace WiSave.Income.Infrastructure.Telemetry;

public static class Extensions
{
    public static IServiceCollection AddTelemetry(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        const string serviceNamespace = "wisave.income";

        var activitySourceName = environment.ApplicationName.ToLowerInvariant();
        var serviceName = activitySourceName.Replace(".", "-");
        var serviceVersion = configuration.GetValue<string>("ServiceVersion") ?? "1.0.0";
        var otlpEndpoint = configuration.GetValue<string>("OpenTelemetry:Endpoint") ?? "http://localhost:4317";
        
        var activitySourceProvider = new ActivitySourceProvider(activitySourceName, serviceVersion);
        services.AddSingleton<IActivitySourceProvider>(activitySourceProvider);
        services.AddScoped<IActivityScope, ActivityScope>();

        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(serviceName, serviceVersion)
            .AddAttributes(new Dictionary<string, object>
            {
                ["service.namespace"] = serviceNamespace,
                ["service.type"] = "worker",
                ["worker.type"] = "domain",
                ["deployment.environment"] = environment.EnvironmentName
            });

        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService(serviceName, serviceVersion)
                .AddAttributes(new Dictionary<string, object>
                {
                    ["service.namespace"] = serviceNamespace,
                    ["service.type"] = "worker",
                    ["worker.type"] = "domain",
                    ["deployment.environment"] = environment.EnvironmentName
                }))
            .WithTracing(tracerBuilder =>
            {
                tracerBuilder
                    .AddSource(activitySourceName)
                    .AddHttpClientInstrumentation(opts => { opts.RecordException = true; })
                    .AddOtlpExporter(opts => { opts.Endpoint = new Uri(otlpEndpoint); });
            })
            .WithMetrics(metricsBuilder =>
            {
                metricsBuilder
                    .AddMeter(activitySourceName)
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddOtlpExporter(opts => { opts.Endpoint = new Uri(otlpEndpoint); });
            });
        
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddOpenTelemetry(options =>
            {
                options.SetResourceBuilder(resourceBuilder);

                options.IncludeFormattedMessage = true;
                options.IncludeScopes = true;
                options.ParseStateValues = true;

                options.AddOtlpExporter(otlpOptions => { otlpOptions.Endpoint = new Uri(otlpEndpoint); });
            });
        });

        return services;
    }
}
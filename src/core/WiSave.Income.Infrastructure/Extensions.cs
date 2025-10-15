using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WiSave.Income.Infrastructure.Configuration;
using WiSave.Income.Infrastructure.EF;
using WiSave.Income.Infrastructure.EventStore;
using WiSave.Income.Infrastructure.Messaging;
using WiSave.Income.Infrastructure.Telemetry;

namespace WiSave.Income.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        return services
            .AddConfiguration(configuration)
            .AddTelemetry(configuration, environment)
            .AddEf()
            .AddEventStore()
            .AddMessaging();
    }
}
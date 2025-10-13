using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WiSave.Income.Infrastructure.Configuration;
using WiSave.Income.Infrastructure.EF;
using WiSave.Income.Infrastructure.EventStore;
using WiSave.Income.Infrastructure.Messaging;

namespace WiSave.Income.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddConfiguration(configuration);

        services.AddEf();
        services.AddEventStore();
        services.AddMessaging();
        
        return services;
    }
}
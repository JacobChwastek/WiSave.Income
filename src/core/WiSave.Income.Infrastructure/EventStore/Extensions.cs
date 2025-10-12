using KurrentDB.Client;
using Microsoft.Extensions.DependencyInjection;
using WiSave.Core.EventStore.Aggregate;
using WiSave.Income.Infrastructure.EventStore.Aggregate;
using WiSave.Shared.Income.Infrastructure.Configuration;

namespace WiSave.Income.Infrastructure.EventStore;

public static class Extensions
{
    public static IServiceCollection AddEventStore(this IServiceCollection services)
    {
        services.AddSingleton<KurrentDBClient>(sp =>
        {
            var config = sp.GetRequiredService<IWiSaveIncomeConfiguration>();
            var connectionString = config.Database.EventStoreDbConnectionString;
            return new KurrentDBClient(KurrentDBClientSettings.Create(connectionString));
        });
    
        services.AddScoped(typeof(IAggregateRepository<,>), typeof(EventStoreAggregateRepository<,>));
    
        return services;
    }
}
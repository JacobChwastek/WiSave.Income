using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WiSave.Shared.Income.Infrastructure.Configuration;

namespace WiSave.Income.Infrastructure.Configuration;

public static class Extension
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var configBuilder = new WiSaveIncomeWorkerDomainConfigurationBuilder(configuration);
        var appConfig = configBuilder.Build();
        
        services.AddSingleton<IWiSaveIncomeConfiguration>(appConfig);
        
        ValidateConfiguration(appConfig);
        
        return services;
    }

    private static void ValidateConfiguration(WiSaveIncomeWorkerDomainConfiguration config)
    {
        var errors = new List<string>();

        if (string.IsNullOrEmpty(config.Database.GeneralDbConnectionString))
            errors.Add("GeneralDb connection string is missing");

        if (string.IsNullOrEmpty(config.Database.EventStoreDbConnectionString))
            errors.Add("EventStoreDb connection string is missing");

        if (string.IsNullOrEmpty(config.RabbitMq.Host))
            errors.Add("RabbitMQ Host is missing");

        if (errors.Any())
        {
            throw new InvalidOperationException(
                $"Configuration validation failed:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
        }
    }
}
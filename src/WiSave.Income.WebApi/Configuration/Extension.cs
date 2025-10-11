using WiSave.Shared.Income.Infrastructure.Configuration;

namespace WiSave.Income.WebApi.Configuration;

internal static class Extension
{
    public static IServiceCollection AddWiSaveConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var configBuilder = new WiSaveIncomeWebApiConfigurationBuilder(configuration);
        var appConfig = configBuilder.Build();
        
        services.AddSingleton<IWiSaveIncomeConfiguration>(appConfig);
        
        ValidateConfiguration(appConfig);

        return services;
    }

    private static void ValidateConfiguration(WiSaveIncomeWebApiConfiguration config)
    {
        var errors = new List<string>();

        if (string.IsNullOrEmpty(config.Database.GeneralDbConnectionString))
            errors.Add("GeneralDb connection string is missing");

        if (string.IsNullOrEmpty(config.Database.ProjectionsDbConnectionString))
            errors.Add("ProjectionsDb connection string is missing");

        if (string.IsNullOrEmpty(config.RabbitMq.Host))
            errors.Add("RabbitMQ Host is missing");

        if (errors.Count != 0)
        {
            throw new InvalidOperationException(
                $"Configuration validation failed:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
        }
    }
}
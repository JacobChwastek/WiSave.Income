using WiSave.Shared.Income.Infrastructure.Configuration;

namespace WiSave.Income.WebApi.Configuration;

public class WiSaveIncomeWebApiConfigurationBuilder(IConfiguration configuration)
{
    public WiSaveIncomeWebApiConfiguration Build()
    {
        return new WiSaveIncomeWebApiConfiguration
        {
            Database = BuildDatabaseConfiguration(),
            RabbitMq = BuildRabbitMqConfiguration(),
            Environment = configuration["ASPNETCORE_ENVIRONMENT"] ?? "Development"
        };
    }

    private DatabaseConfiguration BuildDatabaseConfiguration()
    {
        return new DatabaseConfiguration
        {
            GeneralDbConnectionString = configuration.GetConnectionString("GeneralDb") 
                ?? throw new InvalidOperationException("GeneralDb connection string is not configured"),
            ProjectionsDbConnectionString = configuration.GetConnectionString("ProjectionsDb")
                ?? throw new InvalidOperationException("ProjectionsDb connection string is not configured")
        };
    }

    private RabbitMqConfiguration BuildRabbitMqConfiguration()
    {
        var rabbitMqSection = configuration.GetSection("RabbitMQ");
        
        return new RabbitMqConfiguration
        {
            Host = rabbitMqSection["Host"] 
                ?? throw new InvalidOperationException("RabbitMQ Host is not configured"),
            VirtualHost = rabbitMqSection["VirtualHost"] 
                ?? throw new InvalidOperationException("RabbitMQ VirtualHost is not configured"),
            Username = rabbitMqSection["Username"] 
                ?? throw new InvalidOperationException("RabbitMQ Username is not configured"),
            Password = rabbitMqSection["Password"] 
                ?? throw new InvalidOperationException("RabbitMQ Password is not configured"),
            Port = int.TryParse(rabbitMqSection["Port"], out var port) ? port : 5672
        };
    }
}
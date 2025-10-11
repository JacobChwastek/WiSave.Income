using WiSave.Shared.Income.Infrastructure.Configuration;

namespace WiSave.Income.WebApi.Configuration;

public class WiSaveIncomeWebApiConfiguration : IWiSaveIncomeConfiguration
{
    public DatabaseConfiguration Database { get; init; } = new();
    public RabbitMqConfiguration RabbitMq { get; init; } = new();
    public string Environment { get; init; } = "Development";
}
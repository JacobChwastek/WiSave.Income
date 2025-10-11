using WiSave.Shared.Income.Infrastructure.Configuration;

namespace WiSave.Income.Infrastructure.Configuration;

public class WiSaveIncomeWorkerDomainConfiguration : IWiSaveIncomeConfiguration
{
    public DatabaseConfiguration Database { get; init; } = new();
    public RabbitMqConfiguration RabbitMq { get; init; } = new();
    public string Environment { get; init; } = "Development";
}
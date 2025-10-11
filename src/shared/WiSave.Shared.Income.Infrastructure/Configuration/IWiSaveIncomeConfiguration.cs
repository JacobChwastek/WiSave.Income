namespace WiSave.Shared.Income.Infrastructure.Configuration;

public interface IWiSaveIncomeConfiguration
{
    DatabaseConfiguration Database { get; }
    RabbitMqConfiguration RabbitMq { get; }
    string Environment { get; }
}
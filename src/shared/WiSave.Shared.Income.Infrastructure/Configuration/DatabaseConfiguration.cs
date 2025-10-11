namespace WiSave.Shared.Income.Infrastructure.Configuration;

public class DatabaseConfiguration
{
    public string GeneralDbConnectionString { get; init; } = string.Empty;
    public string ProjectionsDbConnectionString { get; init; } = string.Empty;
    public string EventStoreDbConnectionString { get; init; } = string.Empty;

}
namespace WiSave.Shared.Income.Infrastructure.Configuration;

public class RabbitMqConfiguration
{
    public string Host { get; init; } = string.Empty;
    public string VirtualHost { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public int Port { get; init; } = 5672;
    
    public string GetConnectionString() => $"amqp://{Username}:{Password}@{Host}:{Port}/{VirtualHost}";
}
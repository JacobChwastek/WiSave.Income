# WiSave.Income - Local Development Setup

Quick start guide to run the WiSave.Income solution locally.

## Prerequisites

- .NET 9.0 SDK
- RabbitMQ server running
- SQL Server or compatible database

## Required Configuration

The solution requires these settings to be configured in `appsettings.Development.json` or environment variables:

### Database Connection Strings

```json
{
  "ConnectionStrings": {
    "GeneralDb": "Server=localhost;Database=WiSave_Income_General;Integrated Security=true;TrustServerCertificate=true",
    "ProjectionsDb": "Server=localhost;Database=WiSave_Income_Projections;Integrated Security=true;TrustServerCertificate=true"
  }
}
```

### RabbitMQ Configuration

```json
{
  "RabbitMQ": {
    "Host": "localhost",
    "VirtualHost": "/",
    "Username": "guest",
    "Password": "guest",
    "Port": 5672
  }
}
```

## Environment Variables (Alternative)

Instead of appsettings, you can use environment variables:

```bash
# Database
export ConnectionStrings__GeneralDb="Server=localhost;Database=WiSave_Income_General;Integrated Security=true;TrustServerCertificate=true"
export ConnectionStrings__ProjectionsDb="Server=localhost;Database=WiSave_Income_Projections;Integrated Security=true;TrustServerCertificate=true"

# RabbitMQ
export RabbitMQ__Host="localhost"
export RabbitMQ__VirtualHost="/"
export RabbitMQ__Username="guest"
export RabbitMQ__Password="guest"
export RabbitMQ__Port="5672"
```

## Quick Start

1. **Start RabbitMQ** (using Docker):
   ```bash
   docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
   ```

2. **Set up databases** (create the databases referenced in your connection strings)

3. **Run all components**:
   ```bash
   # Terminal 1 - Web API
   cd src/WiSave.Income.WebApi
   dotnet run

   # Terminal 2 - Domain Worker
   cd src/WiSave.Income.Worker.Domain  
   dotnet run

   # Terminal 3 - Projections Worker
   cd src/WiSave.Income.Worker.Projections
   dotnet run
   ```

4. **Access the API**:
   - OpenAPI docs: https://localhost:7106/scalar/v1 (port may vary)
   - API endpoint: https://localhost:7106/api/incomes

## Docker Alternative

Use the provided `compose.yaml` if available:

```bash
docker compose up
```

## Troubleshooting

- **Configuration errors**: Check that all required settings are present in `appsettings.Development.json`
- **RabbitMQ connection**: Verify RabbitMQ is running on the configured host/port
- **Database connection**: Ensure databases exist and connection strings are correct
- **Port conflicts**: Check `Properties/launchSettings.json` for configured ports

For more details, see the configuration validation in `src/WiSave.Income.WebApi/Configuration/Extension.cs`.
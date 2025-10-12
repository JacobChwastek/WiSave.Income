This file helps AI coding assistants get productive quickly in the WiSave.Income repository.

Follow these focused, actionable rules when making edits or suggesting code:

- Big picture
  - This solution contains three runtime components in `src/`: the Web API (`WiSave.Income.WebApi`), a domain worker (`WiSave.Income.Worker.Domain`) that consumes commands from RabbitMQ, and a projections worker (`WiSave.Income.Worker.Projections`) that hosts background processing.
  - Communication between the API and workers is done via MassTransit over RabbitMQ; the typed bus interface is `WiSave.Shared.Income.Infrastructure.MassTransit.IIncomeBus`.
  - Domain code lives under `src/core/` (Application, Domain, Infrastructure). Contracts and DTOs are under `src/shared/` (`WiSave.Income.Contracts`, `WiSave.Shared.Income.Infrastructure`).

- Build / run / debug (local developer flow)
  - All projects target dotnet — C#. Use the solution or individual project files with `dotnet`.
  - Common commands:
    - Build solution: `dotnet build WiSave.Income.slnx`
    - Run Web API: `cd src/WiSave.Income.WebApi && dotnet run`
    - Run domain worker: `cd src/WiSave.Income.Worker.Domain && dotnet run`
    - Run projections worker: `cd src/WiSave.Income.Worker.Projections && dotnet run`
  - Configuration is read via `IWiSaveIncomeConfiguration` implementations (see `WiSave.Income.WebApi/Configuration/Extension.cs` and `core/WiSave.Income.Infrastructure/Configuration`). When running locally, ensure required connection strings and RabbitMQ settings are present (appsettings*.json or environment variables).

- Important files and examples (refer to these when making changes)
  - `src/WiSave.Income.WebApi/Program.cs` — Web host, OpenAPI setup, MassTransit producer registration, and route mapping (`MapEndpoints`).
  - `src/WiSave.Income.WebApi/Endpoints/IncomeEndpoints.cs` — Example minimal endpoint that publishes a `CreateIncome` command to the bus using `IPublishEndpoint` bound to `IIncomeBus`.
  - `src/WiSave.Income.Worker.Domain/Program.cs` — Worker host that registers MassTransit consumers (e.g., `CreateIncomeCommandHandler`). Use this as the template for adding new consumers.
  - `src/core/WiSave.Income.Application/CommandHandlers/CreateIncomeCommandHandler.cs` — Consumer pattern: implement `IConsumer<T>` and place logic in `Consume(ConsumeContext<T>)`.
  - Contracts: `src/shared/WiSave.Income.Contracts/v1/Commands/CreateIncome.cs` and related DTOs under `v1/Models` — keep contract types backward-compatible.
  - Configuration interface: `src/shared/WiSave.Shared.Income.Infrastructure/Configuration/IWiSaveIncomeConfiguration.cs` — use this for environment-agnostic configuration access.

- Project-specific conventions
  - MassTransit endpoints use a custom `DefaultEndpointNameFormatter` (see Program.cs) and register endpoints via `cfg.ConfigureEndpoints(context)`; prefer consumers over manual ReceiveEndpoint declarations unless required.
  - Use minimal APIs for HTTP surface — endpoints are grouped in `Map{Area}Endpoints` extension methods and registered with `app.MapEndpoints()`.
  - Configuration is validated at startup in extension methods (see `AddWiSaveConfiguration` in WebApi) — ensure new config fields are validated similarly.
  - Domain code is organized into Application (use cases/handlers), Domain (aggregates/entities), and Infrastructure (repos/config). Follow existing folder layout when adding features.

- Integration and external dependencies
  - RabbitMQ via MassTransit (host config read from `IWiSaveIncomeConfiguration`). Be cautious when changing message shapes: update all consumers and projection handlers.
  - Database connection strings are in `IWiSaveIncomeConfiguration.Database` (projections and general DBs). Projections worker will likely use the projections DB.
  - OpenAPI and Scalar integration is used in the Web API (`AddOpenApi()` and `MapScalarApiReference`) — keep the endpoint metadata (Name, Description, Produces) consistent for automatic docs.

- Making code changes
  - Prefer small, focused PRs that update the contracts and then consumers/producers in separate, linked PRs when message shape changes.
  - When adding a new command/event type, add it under `src/shared/WiSave.Income.Contracts/v1/` and update consumers in `WiSave.Income.Worker.Domain` and any publishers in the Web API.
  - Follow the existing DI patterns: register config via `AddWiSaveConfiguration` / `AddConfiguration` extensions and MassTransit via `AddMassTransit<IIncomeBus>(...)`.

- Tests and quality gates
  - There are no top-level test projects visible in `src/` (check before adding). If you add tests, prefer xUnit and keep them colocated in a `tests/` folder or `src/...Tests` sibling projects.

- Quick examples to copy
  - Publish command from HTTP endpoint: see `IncomeEndpoints.CreateIncomeAsync` (uses `Bind<IIncomeBus, IPublishEndpoint>` and `publishEndpoint.Value.Publish(command, token)`).
  - Consumer skeleton: see `CreateIncomeCommandHandler : IConsumer<CreateIncome>` — implement the business logic in `Consume`.

If any section is unclear or you'd like more detail (e.g., build scripts, CI, or message naming conventions), tell me which area to expand and I'll iterate.

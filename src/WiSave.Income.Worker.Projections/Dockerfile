FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/WiSave.Income.Worker.Projections/WiSave.Income.Worker.Projections.csproj", "src/WiSave.Income.Worker.Projections/"]
RUN dotnet restore "src/WiSave.Income.Worker.Projections/WiSave.Income.Worker.Projections.csproj"
COPY . .
WORKDIR "/src/src/WiSave.Income.Worker.Projections"
RUN dotnet build "./WiSave.Income.Worker.Projections.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WiSave.Income.Worker.Projections.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WiSave.Income.Worker.Projections.dll"]

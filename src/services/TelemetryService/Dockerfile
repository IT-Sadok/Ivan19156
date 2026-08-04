FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER root
RUN apt-get update && apt-get install -y libgssapi-krb5-2 && rm -rf /var/lib/apt/lists/*
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/services/TelemetryService/TelemetryService.Rest/TelemetryService.Rest.csproj", "src/services/TelemetryService/TelemetryService.Rest/"]
COPY ["src/services/TelemetryService/TelemetryService.Application/TelemetryService.Application.csproj", "src/services/TelemetryService/TelemetryService.Application/"]
COPY ["src/services/TelemetryService/TelemetryService.Domain/TelemetryService.Domain.csproj", "src/services/TelemetryService/TelemetryService.Domain/"]
COPY ["src/services/TelemetryService/TelemetryService.Infrastructure/TelemetryService.Infrastructure.csproj", "src/services/TelemetryService/TelemetryService.Infrastructure/"]
COPY ["src/services/TelemetryService/TelemetryService.Interfaces/TelemetryService.Interfaces.csproj", "src/services/TelemetryService/TelemetryService.Interfaces/"]
COPY ["shared/IoT.Shared/IoT.Shared.csproj", "shared/IoT.Shared/"]
COPY ["shared/IoT.Contracts/IoT.Contracts.csproj", "shared/IoT.Contracts/"]
COPY ["src/IoT.Domain/IoT.Domain.csproj", "src/IoT.Domain/"]
RUN dotnet restore "src/services/TelemetryService/TelemetryService.Rest/TelemetryService.Rest.csproj"
COPY . .
WORKDIR "/src/src/services/TelemetryService/TelemetryService.Rest"
RUN dotnet build "./TelemetryService.Rest.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TelemetryService.Rest.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TelemetryService.Rest.dll"]
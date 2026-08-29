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
COPY ["src/services/MaintenanceService/MaintenanceService.Rest/MaintenanceService.Rest.csproj", "src/services/MaintenanceService/MaintenanceService.Rest/"]
COPY ["src/services/MaintenanceService/MaintenanceService.Application/MaintenanceService.Application.csproj", "src/services/MaintenanceService/MaintenanceService.Application/"]
COPY ["src/services/MaintenanceService/MaintenanceService.Domain/MaintenanceService.Domain.csproj", "src/services/MaintenanceService/MaintenanceService.Domain/"]
COPY ["src/services/MaintenanceService/MaintenanceService.Infrastructure/MaintenanceService.Infrastructure.csproj", "src/services/MaintenanceService/MaintenanceService.Infrastructure/"]
COPY ["src/services/MaintenanceService/MaintenanceService.Interfaces/MaintenanceService.Interfaces.csproj", "src/services/MaintenanceService/MaintenanceService.Interfaces/"]
COPY ["shared/IoT.Shared/IoT.Shared.csproj", "shared/IoT.Shared/"]
COPY ["shared/IoT.Contracts/IoT.Contracts.csproj", "shared/IoT.Contracts/"]
COPY ["src/IoT.Domain/IoT.Domain.csproj", "src/IoT.Domain/"]
RUN dotnet restore "src/services/MaintenanceService/MaintenanceService.Rest/MaintenanceService.Rest.csproj"
COPY . .
WORKDIR "/src/src/services/MaintenanceService/MaintenanceService.Rest"
RUN dotnet build "./MaintenanceService.Rest.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MaintenanceService.Rest.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MaintenanceService.Rest.dll"]
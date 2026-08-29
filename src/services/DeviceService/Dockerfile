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
COPY ["src/services/DeviceService/DeviceService.Rest/DeviceService.Rest.csproj", "src/services/DeviceService/DeviceService.Rest/"]
COPY ["src/services/DeviceService/DeviceService.Application/DeviceService.Application.csproj", "src/services/DeviceService/DeviceService.Application/"]
COPY ["src/services/DeviceService/DeviceService.Domain/DeviceService.Domain.csproj", "src/services/DeviceService/DeviceService.Domain/"]
COPY ["src/services/DeviceService/DeviceService.Infrastructure/DeviceService.Infrastructure.csproj", "src/services/DeviceService/DeviceService.Infrastructure/"]
COPY ["src/services/DeviceService/DeviceService.Interfaces/DeviceService.Interfaces.csproj", "src/services/DeviceService/DeviceService.Interfaces/"]
COPY ["shared/IoT.Shared/IoT.Shared.csproj", "shared/IoT.Shared/"]
COPY ["shared/IoT.Contracts/IoT.Contracts.csproj", "shared/IoT.Contracts/"]
COPY ["src/IoT.Domain/IoT.Domain.csproj", "src/IoT.Domain/"]
RUN dotnet restore "src/services/DeviceService/DeviceService.Rest/DeviceService.Rest.csproj"
COPY . .
WORKDIR "/src/src/services/DeviceService/DeviceService.Rest"
RUN dotnet build "./DeviceService.Rest.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DeviceService.Rest.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeviceService.Rest.dll"]
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/gateway/IoTGateway/IoTGateway.csproj", "src/gateway/IoTGateway/"]
COPY ["shared/IoT.Shared/IoT.Shared.csproj", "shared/IoT.Shared/"]
COPY ["src/IoT.Domain/IoT.Domain.csproj", "src/IoT.Domain/"]
RUN dotnet restore "src/gateway/IoTGateway/IoTGateway.csproj"
COPY . .
WORKDIR "/src/src/gateway/IoTGateway"
RUN dotnet build "./IoTGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./IoTGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IoTGateway.dll"]
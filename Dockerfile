FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/IoT.Rest/IoT.Rest.csproj", "src/IoT.Rest/"]
COPY ["src/IoT.Application/IoT.Application.csproj", "src/IoT.Application/"]
COPY ["src/IoT.Interfaces/IoT.Interfaces.csproj", "src/IoT.Interfaces/"]
COPY ["src/IoT.Domain/IoT.Domain.csproj", "src/IoT.Domain/"]
COPY ["src/IoT.Contracts/IoT.Contracts.csproj", "src/IoT.Contracts/"]
COPY ["src/IoT.Shared/IoT.Shared.csproj", "src/IoT.Shared/"]
COPY ["src/IoT.Infrastructure/IoT.Infrastructure.csproj", "src/IoT.Infrastructure/"]
RUN dotnet restore "src/IoT.Rest/IoT.Rest.csproj"
COPY . .
WORKDIR "/src/src/IoT.Rest"
RUN dotnet build "./IoT.Rest.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./IoT.Rest.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IoT.Rest.dll"]

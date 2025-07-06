FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
ARG RESOURCE_REAPER_SESSION_ID="00000000-0000-0000-0000-000000000000"
LABEL org.testcontainers.resource-reaper-session=$RESOURCE_REAPER_SESSION_ID
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
EXPOSE 7173

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG RESOURCE_REAPER_SESSION_ID="00000000-0000-0000-0000-000000000000"
LABEL org.testcontainers.resource-reaper-session=$RESOURCE_REAPER_SESSION_ID
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Fluke.CollectorAPI/Fluke.CollectorAPI.csproj", "Fluke.CollectorAPI/"]
RUN dotnet restore "Fluke.CollectorAPI/Fluke.CollectorAPI.csproj"
COPY . .
WORKDIR "/src/Fluke.CollectorAPI"
RUN dotnet build "./Fluke.CollectorAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG RESOURCE_REAPER_SESSION_ID="00000000-0000-0000-0000-000000000000"
LABEL org.testcontainers.resource-reaper-session=$RESOURCE_REAPER_SESSION_ID
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Fluke.CollectorAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
ARG RESOURCE_REAPER_SESSION_ID="00000000-0000-0000-0000-000000000000"
LABEL org.testcontainers.resource-reaper-session=$RESOURCE_REAPER_SESSION_ID
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fluke.CollectorAPI.dll"]

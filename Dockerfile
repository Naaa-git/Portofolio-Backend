# Multi-stage build: the SDK image (big — compilers, NuGet caches) only exists
# during build; the final image only carries the published output plus the
# much smaller ASP.NET Core runtime, so it stays lean for actual deployment.

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy just the project file first so `dotnet restore` (slow, network-bound)
# is cached by Docker and skipped on rebuilds where only the source changed,
# not the dependency list.
COPY Portfolio.Api/Portfolio.Api.csproj Portfolio.Api/
RUN dotnet restore Portfolio.Api/Portfolio.Api.csproj

COPY Portfolio.Api/ Portfolio.Api/
RUN dotnet publish Portfolio.Api/Portfolio.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# The base image already runs as a non-root "app" user and listens on 8080
# by default (since .NET 8) — both are deliberate container security/
# convention defaults, kept as-is rather than overridden.
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080

COPY --from=build /app/publish .

# Migrations + seeding run on startup (see Program.cs) — no separate
# "migrate" step needed before the container is considered ready.
ENTRYPOINT ["dotnet", "Portfolio.Api.dll"]

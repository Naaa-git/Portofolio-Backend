# Portfolio API

ASP.NET Core backend for a personal portfolio site — serves public content
(profile, projects, experience, skills, lessons, outside-code) plus an admin
API for managing it, auth, and search.

## Stack

- ASP.NET Core 10 (.NET 10)
- PostgreSQL 16 — primary database
- OpenSearch 2.19 — full reindex from Postgres on every startup
- Kafka — decouples sending OTP login emails from the login request itself
- Prometheus + Grafana + Loki — metrics, dashboards, structured logs
- Serilog — structured JSON logging

## Prerequisites

- .NET 10 SDK
- Docker + Docker Compose

## Setup (local dev)

1. Clone the repo.
2. Copy `Portfolio.Api/appsettings.Development.json.example` to
   `Portfolio.Api/appsettings.Development.json` and fill in whatever secrets
   you need locally (Brevo API key, Google/Microsoft OAuth client IDs). Most
   of these are only needed if you're testing that specific feature — the
   API runs fine without them for general dev work.
3. `docker compose up -d` — starts Postgres, OpenSearch, Kafka, Prometheus,
   Loki, Mailpit, and Grafana.
4. `dotnet run --project Portfolio.Api` — applies EF Core migrations and
   seeds an admin user automatically on startup.
5. API is up at `http://localhost:5229`, Swagger UI at `/swagger`.

Seeded admin login (dev only, from `SeedAdmin` in
`appsettings.Development.json`): `admin` / `ChangeMe123!`.

## Useful commands

- `dotnet run --project Portfolio.Api` — run the API
- `dotnet test` — run the test suite (`Portfolio.Api.Tests`)
- `dotnet ef migrations add <Name> --project Portfolio.Api` — add a migration
- `dotnet ef database update --project Portfolio.Api` — apply migrations manually

## Project structure

- `Controllers/` — API endpoints
- `Services/` — business logic
- `Repositories/` — data access, one repository per entity
- `Data/` — EF Core `DbContext`, migrations, seed data
- `Models/` — entities, DTOs, validation attributes
- `monitoring/` — Prometheus/Loki/Grafana config
- `docs/migrations/` — rollback notes for breaking DB migrations

## Local observability

- Grafana — `http://localhost:3030` (anonymous admin access in dev only)
- Mailpit (catches outgoing email instead of sending it) — `http://localhost:8025`
- Prometheus — `http://localhost:9090`

Mailpit and anonymous Grafana access are dev-only — production uses real
SMTP and sits Grafana behind auth + a reverse proxy.

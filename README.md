# Warehouse API

A REST API for warehouse inventory management, built with ASP.NET Core and Entity Framework Core. It exposes a controller-based Web API with JWT authentication, owner-based authorization and a clean service layer, backed by SQL Server.

> Part of a full stack learning project — the Flutter client lives in [warehouse-app](https://github.com/scafley/warehouse-app).

## Tech stack

- **.NET 10** / ASP.NET Core Web API (controller-based)
- **Entity Framework Core 10** (code-first migrations)
- **SQL Server** (running in Docker)
- **JWT authentication** + owner-based authorization (BCrypt password hashing)
- **Docker & Docker Compose** (API + database, automatic migrations on startup)
- **OpenAPI** documentation (Scalar UI in Development)
- **Clean architecture:** entities, DTOs, service layer, controllers

## Domain

The API models a small warehouse system:

- **Users** — registration and login, JWT-based auth.
- **Warehouses** — owned by users; a user only sees and manages their own data (many-to-many `UserWarehouse`).
- **Products** — belong to a warehouse and a category (1:N); include a low-stock flag (`quantity < minQuantity`).
- **Categories** — group products (1:N).
- **Stock movements** — inbound/outbound operations with business rules (e.g. an outbound movement cannot exceed the current stock), returning a result object rather than throwing.

All product, warehouse and movement data is filtered by the authenticated user — the user id is always taken from the JWT, never from the request body.

## Architecture

The project separates concerns across distinct layers:

- **Entities** (`Models/`) — database-mapped models, managed by EF Core.
- **DTOs** (`Models/`) — request/response contracts, decoupled from entities so the database schema and the public API can evolve independently.
- **Service layer** (`Services/`) — business logic and data access, with no knowledge of HTTP. Expected outcomes are returned as plain results (`null` / `bool` / result objects) rather than exceptions.
- **Controllers** (`Controllers/`) — thin HTTP layer that maps service results to status codes; `[Authorize]` protects the domain endpoints.

## API overview

| Area | Example routes | Auth |
|------|---------------|------|
| Auth | `POST /api/auth/register`, `POST /api/auth/login` | public |
| Warehouses | `GET`/`POST /api/warehouses` | JWT |
| Products | `GET`/`POST /api/products` (filterable by `?warehouseId=`), `GET`/`PUT`/`DELETE /api/products/{id}` | JWT |
| Stock movements | `GET`/`POST /api/products/{id}/movements` | JWT |

Full, interactive documentation is available via Scalar at `/scalar/v1` when running in Development.

## Getting started

### Prerequisites

- [Docker](https://www.docker.com/) (Docker Compose)

That's the only requirement — the database, migrations and API all run in containers.

### 1. Configure secrets

Copy the example env file and fill in your own values:

```bash
cp .env.example .env
```

```env
DB_PASSWORD=YourStrongPassword123
JWT_KEY=your-jwt-signing-key-min-32-characters
```

> `.env` is git-ignored; only `.env.example` is committed. The JWT key must be at least 32 characters.

### 2. Run

```bash
docker compose up --build
```

This starts SQL Server and the API, and applies EF Core migrations automatically on startup. The API will be available at `http://localhost:8080`.

To reset the database (removes all data):

```bash
docker compose down -v
```

## Notes

- **Secrets** are kept out of version control — they live in `.env` (git-ignored and docker-ignored), injected via environment variables.
- **Business errors** (e.g. insufficient stock, resource not found) are handled through the service layer's result values, not exceptions.
- **Authorization** is owner-based across the whole domain: every query is scoped to the authenticated user via their warehouses.

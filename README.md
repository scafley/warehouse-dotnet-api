# Warehouse API

A REST API for warehouse inventory management, built with ASP.NET Core and Entity Framework Core. Products are stored in SQL Server and exposed through a controller-based Web API with a clean service layer.

## Tech stack

- **.NET 10** / ASP.NET Core Web API (controller-based)
- **Entity Framework Core 10** (code-first migrations)
- **SQL Server** (running in Docker)
- **Clean architecture:** entities, DTOs, service layer, controllers

## Architecture

The project separates concerns across distinct layers:

- **Entities** (`Models/Product.cs`) — database-mapped models, managed by EF Core.
- **DTOs** (`Models/ProductDtos.cs`) — request/response contracts, decoupled from entities so the database schema and the public API can evolve independently.
- **Service layer** (`Services/`) — business logic and data access, exposed through `IProductService`. Returns plain results (`null` / `bool`), with no knowledge of HTTP.
- **Controllers** (`Controllers/`) — thin HTTP layer that maps service results to status codes.

## API endpoints

| Method | Route | Description | Success |
|--------|-------|-------------|---------|
| GET | `/api/products` | List all products | 200 |
| GET | `/api/products/{id}` | Get product by id | 200 / 404 |
| POST | `/api/products` | Create a product | 201 |
| PUT | `/api/products/{id}` | Update a product | 204 / 404 |
| DELETE | `/api/products/{id}` | Delete a product | 204 / 404 |

### Product model

| Field | Type | Notes |
|-------|------|-------|
| `id` | int | Auto-generated |
| `name` | string | Required |
| `sku` | string | Stock keeping unit / barcode |
| `quantity` | int | Current stock level |
| `minQuantity` | int | Minimum stock threshold |
| `createdAt` | datetime | Set on creation |

### Example request

```http
POST /api/products
Content-Type: application/json

{
  "name": "Wireless mouse",
  "sku": "MX-001",
  "quantity": 10,
  "minQuantity": 3
}
```

## Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) (for SQL Server)

### 1. Start the database

```bash
docker run -e ACCEPT_EULA=Y -e MSSQL_SA_PASSWORD=YourPassword123 \
  -p 1434:1433 --name warehouse-db -d mcr.microsoft.com/mssql/server:2022-latest
```

> On Apple Silicon, add `--platform linux/amd64` (the image runs under emulation).

### 2. Configure the connection string

This repo ships `appsettings.json` with a placeholder password. Create `appsettings.Development.json` (git-ignored) with your real connection string:

```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost,1434;Database=warehouse;User Id=sa;Password=YourPassword123;TrustServerCertificate=True"
  }
}
```

### 3. Apply migrations

```bash
dotnet ef database update
```

### 4. Run

```bash
dotnet run
```

The API will be available at `http://localhost:5145`.

## Notes

- Validation is handled in the controllers (e.g. product name is required).
- Secrets are kept out of version control — the real connection string lives in `appsettings.Development.json`, which is git-ignored.
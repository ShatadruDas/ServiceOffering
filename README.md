# Service Offering API

Service Offering is a .NET 10 Web API for managing products, customers, and orders. It uses Entity Framework Core with SQL Server LocalDB for persistence and exposes Swagger/OpenAPI documentation in Development.

## Repository Layout

```text
ServiceOffering/
|- Sd.ServiceOffering.Api/
|  |- Sd.ServiceOffering.Api/             HTTP API and controllers
|  |- Sd.SolutionOffering.Domain/         Domain entities, ports, and use cases
|  |- Sd.SolutionOffering.Adapter/        EF Core database adapter and repositories
|  `- Sd.ServiceOffering.Api.slnx          API solution
`- Sd.ServiceOffering.Database/            SQL Server database project and scripts
```

## Architecture

The API follows Ports and Adapters (Hexagonal Architecture):

```text
HTTP request
    |
    v
API controllers (inbound adapter)
    |
    v
Domain inbound ports and application services
    |
    v
Domain outbound repository and Unit of Work ports
    |
    v
EF Core database adapter (outbound adapter)
    |
    v
SQL Server LocalDB
```

### Domain Layer

`Sd.SolutionOffering.Domain` contains:

- Entities: `Product`, `Customer`, `Order`, and `OrderProduct`.
- Inbound ports: `IProductService`, `ICustomerService`, and `IOrderService`.
- Outbound ports: repository interfaces and `IUnitOfWork`.
- Application services that enforce basic use-case validation and commit changes.

The Domain project has no dependency on ASP.NET Core, EF Core, or SQL Server. This follows Dependency Inversion: business rules depend on abstractions rather than infrastructure.

### Database Adapter

`Sd.SolutionOffering.Adapter` implements the outbound ports with:

- `ServiceOfferingDbContext` for EF Core table mappings.
- `SqlProductRepository`, `SqlCustomerRepository`, and `SqlOrderRepository`.
- `SqlUnitOfWork` for scoped EF transactions.
- LINQ queries with `AsNoTracking`, `FirstOrDefaultAsync`, and `ToListAsync` for reads.

Order creation persists the order and its associated products in one Unit of Work transaction. The relationship uses `OrderProduct` and stores the unit price captured at order time.

### API Layer

The API controllers are inbound adapters and depend only on Domain inbound ports:

- `GET/POST /api/products`
- `GET/POST /api/customers`
- `GET/POST /api/orders/{orderNumber}`

Swagger UI is available at `/swagger` when the application runs in Development.

## Prerequisites

- .NET SDK 10.0 or later.
- SQL Server LocalDB with the `MSSQLLocalDB` instance.
- Visual Studio 2022/2026 or another editor with .NET tooling.
- SQL Server Data Tools if you need to build or publish the separate SQL project.

## Database Setup

The API currently uses this connection string in `appsettings.json`:

```text
Server=(localdb)\\MSSQLLocalDB;Database=ServiceOfferingDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True
```

Start LocalDB if needed:

```powershell
sqllocaldb start MSSQLLocalDB
```

Create or publish the database using the SQL project in `Sd.ServiceOffering.Database`. The SQL project contains the table definitions for products, pricing, customers, orders, and order-product relationships.

For local development, do not commit machine-specific connection strings or credentials. Prefer User Secrets or environment variables when the connection differs from the checked-in development value.

## Build and Run

From the repository root:

```powershell
dotnet restore .\Sd.ServiceOffering.Api\Sd.ServiceOffering.Api.slnx
dotnet build .\Sd.ServiceOffering.Api\Sd.ServiceOffering.Api.slnx
dotnet run --project .\Sd.ServiceOffering.Api\Sd.ServiceOffering.Api\Sd.ServiceOffering.Api.csproj
```

Then open:

- Swagger: `https://localhost:7107/swagger`
- HTTP API: `http://localhost:5042`

The exact ports can vary with `Properties/launchSettings.json`.

## Development Guidelines

- Keep business rules in Domain services, not controllers or repositories.
- Add new persistence capabilities through an outbound port before implementing them in the adapter.
- Keep EF Core models and mappings inside the database adapter.
- Use the Unit of Work for multi-table writes that must commit atomically.
- Pass `CancellationToken` through API, Domain, and persistence calls.
- Add tests for new use cases and repository behavior before changing shared contracts.

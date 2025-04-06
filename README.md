# Lumora Cosmetics Marketplace with ASP.NET 8 with ADO.NET and PostgreSQL Backend

This document describes the architecture and organization of the backend solution built with ASP.NET 8, ADO.NET for data access, and PostgreSQL as the database.

## Project Structure
```text
/Backend
│
├── /src
│ ├── /Presentation
│ │ ├── /API
│ │ │ ├── /Controllers
│ │ │ ├── Program.cs
│ │ │ ├── appsettings.json
│ │ │ ├── Startup.cs
│ │ │ └── SwaggerSetup.cs
│ │ └── /wwwroot
│ │
│ ├── /Application
│ │ ├── /Interfaces
│ │ ├── /DTOs
│ │ ├── /Services
│ │ └── /Validators
│ │
│ ├── /Domain
│ │ ├── /Entities
│ │ ├── /ValueObjects
│ │ └── /Enums
│ │
│ ├── /Infrastructure
│ ├── /Persistence
│ │ ├── /Repositories
│ │ ├── /Configurations
│ │ └── DatabaseContext.cs
│ ├── /Services
│ └── appsettings.Development.json
│
├── /tests
│ ├── /UnitTests
│ 
│
└── README.md
```

## Layer Responsibilities

### 1. Presentation Layer (API)
- **Location**: `src/Presentation/API`
- **Responsibilities**:
  - HTTP request handling
  - Authentication/authorization
  - Input validation
  - Response formatting
- **Key Components**:
  - `Controllers`: Handle HTTP requests
  - `Program.cs`: Application entry point and service configuration
  - `appsettings.json`: Configuration files

### 2. Application Layer
- **Location**: `src/Application`
- **Responsibilities**:
  - Business logic implementation
  - DTO definitions
  - Validation rules
  - Mediation between Presentation and Domain layers
- **Key Components**:
  - `Services`: Business logic implementations
  - `DTOs`: Data Transfer Objects
  - `Interfaces`: Service contracts
  - `Validators`: FluentValidation rules

### 3. Domain Layer
- **Location**: `src/Domain`
- **Responsibilities**:
  - Core business entities
  - Domain rules
  - Value objects
  - Repository interfaces
- **Key Components**:
  - `Entities`: Core business objects
  - `ValueObjects`: Domain value objects
  - `Enums`: Domain enumerations
  - `Interfaces`: Repository contracts

### 4. Infrastructure Layer
- **Location**: `src/Infrastructure`
- **Responsibilities**:
  - Data persistence implementation
  - External service integrations
  - Cross-cutting concerns
- **Key Components**:
  - `Persistence`: ADO.NET and PostgreSQL implementations
  - `Services`: Third-party service integrations
  - `DatabaseContext`: PostgreSQL connection management

## PostgreSQL Data Access

The project uses ADO.NET with Npgsql for PostgreSQL data access:

```csharp
// Example repository implementation
public class SampleRepository : ISampleRepository
{
    private readonly DatabaseContext _context;

    public SampleRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Sample>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();
        var sql = "SELECT * FROM samples";
        return await connection.QueryAsync<Sample>(sql);
    }
}

# Family Safety Application

A comprehensive family safety solution built with .NET 10 and Clean Architecture principles. This application provides real-time location tracking, geofencing, safety alerts, and family group management.

## 🏗️ Architecture

This project follows **Clean Architecture** principles with the following layers:

```
FamilySafety/
├── src/
│   ├── FamilySafety.API/           # Web API layer (Controllers, Middleware)
│   ├── FamilySafety.Application/   # Business logic, CQRS with MediatR
│   ├── FamilySafety.Domain/        # Entities, Value Objects, Domain Events
│   ├── FamilySafety.Infrastructure/# EF Core, External Services
│   └── FamilySafety.Shared/        # Common utilities, DTOs, Constants
├── tests/
│   ├── FamilySafety.UnitTests/
│   └── FamilySafety.IntegrationTests/
└── FamilySafety.sln
```

## 🛠️ Technology Stack

- **.NET 10 / ASP.NET Core 10** - Latest .NET framework
- **Entity Framework Core** - ORM with SQL Server
- **MediatR** - CQRS pattern implementation
- **FluentValidation** - Request validation
- **AutoMapper** - Object mapping
- **Serilog** - Structured logging
- **Swashbuckle** - Swagger/OpenAPI documentation
- **Azure Services**:
  - Azure AD B2C (Authentication)
  - Azure SignalR (Real-time communication)
  - Azure Notification Hubs (Push notifications)
  - Azure Blob Storage (File storage)

## 📋 Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server](https://www.microsoft.com/sql-server) or SQL Server LocalDB
- [Azure Account](https://azure.microsoft.com) (for Azure services)
- IDE: [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd FamilySafety
```

### 2. Configure the Application

Update `src/FamilySafety.API/appsettings.json` with your configuration:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your SQL Server connection string"
  },
  "AzureAdB2C": {
    "Instance": "https://your-tenant.b2clogin.com",
    "Domain": "your-tenant.onmicrosoft.com",
    "ClientId": "your-client-id"
  }
}
```

### 3. Apply Database Migrations

```bash
cd src/FamilySafety.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../FamilySafety.API
dotnet ef database update --startup-project ../FamilySafety.API
```

### 4. Run the Application

```bash
cd src/FamilySafety.API
dotnet run
```

The API will be available at:

- HTTP: <http://localhost:5000>
- HTTPS: <https://localhost:5001>
- Swagger UI: <https://localhost:5001/swagger> (in development)

## 📚 API Endpoints

### Users

- `GET /api/v1/users/me` - Get current user profile
- `PUT /api/v1/users/me` - Update user profile
- `PUT /api/v1/users/me/device-token` - Update device token

### Family Groups

- `GET /api/v1/familygroups` - Get user's family groups
- `POST /api/v1/familygroups` - Create a new family group
- `POST /api/v1/familygroups/join` - Join using invite code
- `GET /api/v1/familygroups/{id}/members` - Get family members

### Locations

- `POST /api/v1/locations` - Update location
- `GET /api/v1/locations/family/{familyGroupId}` - Get family locations
- `GET /api/v1/locations/history/{userId}` - Get location history

### Alerts

- `POST /api/v1/alerts/sos` - Trigger SOS alert
- `POST /api/v1/alerts/panic` - Trigger panic button
- `POST /api/v1/alerts/{id}/acknowledge` - Acknowledge alert
- `POST /api/v1/alerts/{id}/resolve` - Resolve alert

### Geofences

- `GET /api/v1/geofences/family/{familyGroupId}` - Get family geofences
- `POST /api/v1/geofences` - Create geofence
- `PUT /api/v1/geofences/{id}` - Update geofence
- `DELETE /api/v1/geofences/{id}` - Delete geofence

## 🧪 Running Tests

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/FamilySafety.UnitTests
dotnet test tests/FamilySafety.IntegrationTests
```

## 📁 Project Structure Details

### Domain Layer (`FamilySafety.Domain`)

Contains enterprise business rules:

- **Entities**: User, FamilyGroup, FamilyMember, Geofence, LocationHistory, SafetyAlert
- **Value Objects**: GeoLocation, Address
- **Domain Events**: SafetyAlertCreatedEvent, UserJoinedFamilyEvent, GeofenceBreachEvent
- **Enums**: FamilyRole, AlertType, AlertStatus, GeofenceType

### Application Layer (`FamilySafety.Application`)

Contains application business rules:

- **CQRS Handlers**: Commands and Queries using MediatR
- **Behaviors**: Validation, Logging pipeline behaviors
- **Interfaces**: Repository, UnitOfWork, External services

### Infrastructure Layer (`FamilySafety.Infrastructure`)

Contains external concerns:

- **Persistence**: EF Core DbContext, Configurations, Repositories
- **Services**: DateTime service, External service implementations

### API Layer (`FamilySafety.API`)

Contains presentation concerns:

- **Controllers**: REST API endpoints
- **Middleware**: Exception handling, Logging
- **Configuration**: Swagger, Authentication, Health checks

## 🔐 Security

- JWT Bearer authentication with Azure AD B2C
- Role-based authorization (Admin, Parent, Guardian, Child, Member)
- Soft delete for data protection
- Request validation using FluentValidation

## 📝 License

This project is licensed under the MIT License.

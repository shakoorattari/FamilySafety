# Family Safety Application - Development Instructions

## Project Overview
This is a comprehensive Family Safety Application built with .NET 10 and Clean Architecture principles.

## Technology Stack
- .NET 10 / ASP.NET Core 10
- Entity Framework Core with SQL Server
- MediatR for CQRS pattern
- FluentValidation for validation
- AutoMapper for object mapping
- Serilog for logging
- Azure services (AD B2C, SignalR, Notification Hubs, Blob Storage)
- JWT Bearer authentication
- Swagger/OpenAPI documentation

## Architecture
- Clean Architecture
- CQRS (Command Query Responsibility Segregation)
- Repository Pattern
- Unit of Work Pattern
- Domain-Driven Design principles

## Project Structure
```
FamilySafety/
├── src/
│   ├── FamilySafety.API/           # Web API layer
│   ├── FamilySafety.Application/   # Business logic, CQRS
│   ├── FamilySafety.Domain/        # Entities, Value Objects
│   ├── FamilySafety.Infrastructure/# EF Core, External Services
│   └── FamilySafety.Shared/        # Common utilities, DTOs
├── tests/
│   ├── FamilySafety.UnitTests/
│   └── FamilySafety.IntegrationTests/
└── FamilySafety.sln
```

## Core Features
1. User Management with Azure AD B2C
2. Family Group Management
3. Location Tracking with Geofencing
4. Safety Alerts (SOS, Panic Button)
5. Activity Monitoring
6. Push Notifications

## Development Guidelines
- Follow Clean Architecture principles
- Use CQRS pattern for all operations
- Implement proper validation using FluentValidation
- Use dependency injection throughout
- Write unit tests for business logic
- Document APIs with Swagger attributes

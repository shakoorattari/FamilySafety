using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FamilySafety.Application.Common.Interfaces;
using FamilySafety.Infrastructure.Persistence;
using FamilySafety.Infrastructure.Persistence.Repositories;
using FamilySafety.Infrastructure.Services;

namespace FamilySafety.Infrastructure;

/// <summary>
/// Dependency injection configuration for the Infrastructure layer
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        // Register repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register services
        services.AddScoped<IDateTimeService, DateTimeService>();

        return services;
    }
}

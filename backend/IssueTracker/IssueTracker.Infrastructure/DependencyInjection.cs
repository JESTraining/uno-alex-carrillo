using IssueTracker.Application.Abstractions;
using IssueTracker.Infrastructure.Persistence;
using IssueTracker.Infrastructure.Repositories;
using IssueTracker.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IssueTracker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString(
                    "DefaultConnection"));
        });

        // Register DbContext as IAppDbContext for Application layer
        services.AddScoped<IAppDbContext>(sp =>
            sp.GetRequiredService<AppDbContext>());

        // Register generic repository implementing Application abstract interface
        services.AddScoped(
            typeof(IRepository<>),
            typeof(Repository<>));

        // Register file storage service
        services.AddScoped<
            Application.Abstractions.IFileStorageService,
            LocalFileStorageService>();

        return services;
    }
}

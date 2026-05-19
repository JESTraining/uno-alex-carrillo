using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using IssueTracker.Application.Interfaces;
using IssueTracker.Application.Services;

namespace IssueTracker.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddAutoMapper(
            typeof(DependencyInjection).Assembly);

        services.AddValidatorsFromAssembly(
            typeof(DependencyInjection).Assembly,
            includeInternalTypes: true);

        services.AddScoped<IIssueService, IssueService>();

        return services;
    }
}

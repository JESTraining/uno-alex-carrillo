using Microsoft.OpenApi.Models;
using System.Reflection;

namespace IssueTracker.Api.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerDocumentation(
        this IServiceCollection services)
    {
        services.AddOpenApi();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Issue Tracker API",
                Version = "v1",
                Description = "REST API for tracking issues, bugs, and tasks with attachment support",
                Contact = new OpenApiContact
                {
                    Name = "Development Team",
                    Email = "dev@issuetracker.com"
                }
            });

            // Add JWT authentication support
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme.",
                In = ParameterLocation.Header
            };

            options.AddSecurityDefinition("Bearer", securityScheme);

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
            };

            options.AddSecurityRequirement(securityRequirement);

            // Add XML documentation
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }

            // Support for file upload
            options.OperationFilter<FileUploadOperationFilter>();
        });

        return services;
    }

    public static WebApplication UseSwaggerDocumentation(
        this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Issue Tracker API v1");
            options.RoutePrefix = string.Empty;
            options.DefaultModelsExpandDepth(2);
        });

        return app;
    }
}

/// <summary>
/// Custom operation filter to properly document file upload endpoints
/// </summary>
public class FileUploadOperationFilter : Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter
{
    public void Apply(
        OpenApiOperation operation,
        Swashbuckle.AspNetCore.SwaggerGen.OperationFilterContext context)
    {
        var fileUploadMime = "multipart/form-data";

        if (operation.RequestBody == null)
            return;

        if (!operation.RequestBody.Content.Any(x => x.Key.Equals(fileUploadMime, StringComparison.OrdinalIgnoreCase)))
            return;

        var fileParameter = context.ApiDescription.ActionDescriptor.Parameters
            .FirstOrDefault(p => p.Name.Equals("file", StringComparison.OrdinalIgnoreCase));

        if (fileParameter == null)
            return;

        operation.RequestBody.Content[fileUploadMime].Schema = new OpenApiSchema
        {
            Type = "object",
            Properties = new Dictionary<string, OpenApiSchema>
            {
                ["file"] = new OpenApiSchema
                {
                    Type = "string",
                    Format = "binary",
                    Description = "Upload file (jpg, jpeg, png - max 5MB)"
                }
            },
            Required = new HashSet<string> { "file" }
        };
    }
}

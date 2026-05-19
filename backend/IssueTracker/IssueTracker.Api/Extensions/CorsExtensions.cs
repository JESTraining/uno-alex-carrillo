namespace IssueTracker.Api.Extensions;

public static class CorsExtensions
{
    public static IServiceCollection AddCorsPolicy(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            // Alternative: configure specific origins for production
            var allowedOrigins = configuration
                .GetSection("AllowedOrigins")
                .Get<string[]>() ?? [];

            if (allowedOrigins.Length > 0)
            {
                options.AddPolicy("AllowSpecific", policy =>
                {
                    policy
                        .WithOrigins(allowedOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            }
        });

        return services;
    }

    public static WebApplication UseCorsPolicy(
        this WebApplication app,
        IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseCors("AllowAll");
        }
        else
        {
            app.UseCors("AllowSpecific");
        }

        return app;
    }
}

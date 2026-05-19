using IssueTracker.Application;
using IssueTracker.Infrastructure;
using IssueTracker.Api.Extensions;
using IssueTracker.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

// Application layer
builder.Services.AddApplication();

// Infrastructure layer
builder.Services.AddInfrastructure(builder.Configuration);

// API layer - Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddJwtTokenGeneration(builder.Configuration);

// API layer - CORS
builder.Services.AddCorsPolicy(builder.Configuration);

// API layer - Swagger/OpenAPI
builder.Services.AddSwaggerDocumentation();

// Controllers
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline

// Global exception handling middleware
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

// Swagger documentation
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

// CORS
app.UseCorsPolicy(app.Environment);

// HTTPS redirect
app.UseHttpsRedirection();

// Authentication and Authorization
app.UseAuthentication();

app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();

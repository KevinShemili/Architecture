using Application.DependencyConfigurations;
using FluentValidation.AspNetCore;
using Infrastructure.DependencyConfigurations;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services
	.AddApplicationLayer(builder.Configuration)
	.AddInfrastructureLayer(builder.Configuration);

builder.Configuration
	.AddJsonFile("appsettings.json")
	.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

builder.Services.AddAutoMapper(
	Assembly.GetExecutingAssembly());

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Call here if you want to apply migrations directly when starting application
//ApplyMigrations(app); 

ConfigureSwagger(app);

app.UseAuthorization();

app.UseAuthentication();

app.UseExceptionHandler();

app.MapControllers();

app.Run();

#pragma warning disable CS8321 // Local function is declared but never used
static async void ApplyMigrations(WebApplication app) {
	// automatically apply migrations
	using var scope = app.Services.CreateScope();
	var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
	await db.Database.MigrateAsync();
}
#pragma warning restore CS8321 // Local function is declared but never used

static void ConfigureSwagger(WebApplication app) {
	app.UseSwagger();
	app.UseSwaggerUI();
}
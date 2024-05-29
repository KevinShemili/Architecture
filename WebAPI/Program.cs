using Application.DependencyConfigurations;
using Domain.Entities.IdentityExtensions;
using Infrastructure.DependencyConfigurations;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddApplicationLayer(builder.Configuration)
	.AddInfrastructureLayer(builder.Configuration);

builder.Configuration
	.AddJsonFile("appsettings.json")
	.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication()
				.AddCookie(IdentityConstants.ApplicationScheme);

var app = builder.Build();

// Call here if you want to apply migrations directly when starting application
//ApplyMigrations(app); 

ConfigureSwagger(app);

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.MapIdentityApi<User>();

app.Run();

static async void ApplyMigrations(WebApplication app) {
	// automatically apply migrations
	using var scope = app.Services.CreateScope();
	var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
	await db.Database.MigrateAsync();
}

static void ConfigureSwagger(WebApplication app) {
	app.UseSwagger();
	app.UseSwaggerUI();
}
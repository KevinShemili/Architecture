using Application;
using Infrastructure;
using Infrastructure.Persistence;
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

var app = builder.Build();

ApplyMigrations(app);
ConfigureSwagger(app);

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();

static void ApplyMigrations(WebApplication app) {
	// automatically apply migrations
	using var scope = app.Services.CreateScope();
	var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
	db.Database.MigrateAsync();
}

static void ConfigureSwagger(WebApplication app) {
	app.UseSwagger();
	app.UseSwaggerUI();
}
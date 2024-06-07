using Application.Contracts.Email;
using Application.Contracts.Persistence;
using Infrastructure.Persistence;
using Infrastructure.Services.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyConfigurations
{
    public static class Dependencies
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.ConfigureScopedServices();
            services.ConfigureDatabaseConnection(configuration);

            return services;
        }

        private static void ConfigureScopedServices(this IServiceCollection services)
        {
            services.AddScoped<ICoreDbContext, CoreDbContext>();
            services.AddScoped<IEmailService, EmailService>();
        }

        private static void ConfigureDatabaseConnection(this IServiceCollection services, IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString("DbConnection");

            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(connString, b => b.MigrationsAssembly("Infrastructure")));
        }
    }
}

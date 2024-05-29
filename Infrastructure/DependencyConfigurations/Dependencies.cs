using Application.Contracts.Persistence;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyConfigurations
{
    public static class Dependencies
    {

        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            ConfigureDatabaseConnection(services, configuration);
            services.ConfigureScopedServices();

            return services;
        }

        private static void ConfigureScopedServices(this IServiceCollection services)
        {
            services.AddScoped<ICoreDbContext, CoreDbContext>();
        }

        private static void ConfigureDatabaseConnection(IServiceCollection services, IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString("DbConnection");
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(connString, b => b.MigrationsAssembly("Infrastructure")));
        }
    }
}

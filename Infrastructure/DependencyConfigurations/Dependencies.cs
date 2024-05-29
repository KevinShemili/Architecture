using Application.Contracts.Persistence;
using Domain.Entities.IdentityExtensions;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyConfigurations
{
    public static class Dependencies
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureScopedServices();
            services.ConfigureASPIdentity();
            services.ConfigureDatabaseConnection(configuration);

            return services;
        }

        private static void ConfigureScopedServices(this IServiceCollection services)
        {
            services.AddScoped<ICoreDbContext, CoreDbContext>();
        }

        private static void ConfigureDatabaseConnection(this IServiceCollection services, IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString("DbConnection");
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(connString, b => b.MigrationsAssembly("Infrastructure")));
        }

        private static void ConfigureASPIdentity(this IServiceCollection services)
        {
            services.AddIdentityCore<User>()
                    .AddEntityFrameworkStores<DatabaseContext>()
                    .AddApiEndpoints();
        }
    }
}

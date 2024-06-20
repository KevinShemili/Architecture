using Application.Contracts.Email;
using Application.Contracts.Persistence;
using Application.Contracts.Token;
using Infrastructure.Authorization;
using Infrastructure.Persistence;
using Infrastructure.Services.Email;
using Infrastructure.Services.Token;
using Microsoft.AspNetCore.Authorization;
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
            services.ConfigureAuthorizationPolicy();

            return services;
        }

        private static void ConfigureScopedServices(this IServiceCollection services)
        {
            services.AddScoped<ICoreDbContext, CoreDbContext>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITokenService, TokenService>();
        }

        private static void ConfigureDatabaseConnection(this IServiceCollection services, 
            IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString("DbConnection");

            services.AddDbContext<DatabaseContext>(options => 
                options.UseSqlServer(connString, b => b.MigrationsAssembly("Infrastructure")));
        }

        private static void ConfigureAuthorizationPolicy(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        }
    }
}

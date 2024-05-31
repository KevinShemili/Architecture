using Application.Contracts.Email;
using Application.Contracts.Persistence;
using Domain.Entities.IdentityExtensions;
using Infrastructure.Persistence;
using Infrastructure.Services.Email;
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
            services.AddScoped<IEmailService, EmailService>();
        }

        private static void ConfigureDatabaseConnection(this IServiceCollection services, IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString("DbConnection");
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(connString, b => b.MigrationsAssembly("Infrastructure")));
        }

        private static void ConfigureASPIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>()
                    .AddEntityFrameworkStores<DatabaseContext>()
                    .AddDefaultTokenProviders()
                    .AddApiEndpoints();

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 8;
                options.Lockout.AllowedForNewUsers = true;

                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 1;

                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                options.User.RequireUniqueEmail = true;
            });
        }
    }
}

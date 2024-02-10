using Application.Contracts.Persistence;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure {
	public static class DependencyInjection {

		public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration) {
			ConfigureDatabaseConnection(services, configuration);
			ConfigureScopedServices(services);

			return services;
		}

		private static void ConfigureScopedServices(this IServiceCollection services) {
			services.AddScoped<ICoreDbContext, CoreDbContext>();
		}

		private static void ConfigureDatabaseConnection(IServiceCollection services, IConfiguration configuration) {
			var connString = configuration.GetConnectionString("DbConnection");
			services.AddDbContext<DatabaseContext>(options =>
				options.UseSqlServer(connString, b => b.MigrationsAssembly("Infrastructure")));
		}
	}
}

using Application.Generic;
using Application.UseCases.Authentication.Commands;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application {
	public static class DependencyInjection {
		public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration) {
			ConfigureMediatR(services);
			ConfigureFluentValidation(services);
			ConfigureAutoMapper(services);
			ConfigureScopedServices(services);

			return services;
		}

		private static void ConfigureMediatR(IServiceCollection services) {
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
		}

		private static void ConfigureFluentValidation(IServiceCollection services) {
			services.AddValidatorsFromAssemblyContaining<SignUpCommand>()
					.AddFluentValidationAutoValidation()
					.AddFluentValidationClientsideAdapters();
		}

		private static void ConfigureAutoMapper(IServiceCollection services) {
			services.AddAutoMapper(Assembly.GetExecutingAssembly());
		}

		private static void ConfigureScopedServices(IServiceCollection services) {
			services.AddScoped<BaseHandlerService>();
		}
	}
}

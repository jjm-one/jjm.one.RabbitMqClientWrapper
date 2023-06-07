using jjm.one.RabbitMqClientWrapper.main.core;
using jjm.one.RabbitMqClientWrapper.types;
using Microsoft.Extensions.DependencyInjection;

namespace jjm.one.RabbitMqClientWrapper.di.core
{
	public static class RMQCCoreDi
	{
		public static IServiceCollection AddRMQCCore(this IServiceCollection services, Settings settings, bool enableCoreLogging = false)
		{
			services.AddSingleton(settings);
			services.AddSingleton(new DiSimpleTypeWrappersEnableCoreLogging(enableCoreLogging));
			services.AddScoped<IRMQCCore, RMQCCore>();

			return services;
		}

    }
}


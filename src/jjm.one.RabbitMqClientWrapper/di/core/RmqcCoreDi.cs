using jjm.one.RabbitMqClientWrapper.main.core;
using jjm.one.RabbitMqClientWrapper.types;
using jjm.one.RabbitMqClientWrapper.types.di;
using Microsoft.Extensions.DependencyInjection;

namespace jjm.one.RabbitMqClientWrapper.di.core
{
	public static class RmqcCoreDi
    {
		public static IServiceCollection AddRMQCCore(this IServiceCollection services, Settings settings, bool enableCoreLogging = false)
		{
			services.AddSingleton(settings);
			services.AddSingleton(new DiSimpleTypeWrappersEnableCoreLogging(enableCoreLogging));
			services.AddScoped<IRmqcCore, RmqcCore>();

			return services;
		}

    }
}


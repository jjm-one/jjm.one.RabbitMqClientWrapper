using jjm.one.RabbitMqClientWrapper.main.core;
using jjm.one.RabbitMqClientWrapper.types;
using jjm.one.RabbitMqClientWrapper.types.di;
using Microsoft.Extensions.DependencyInjection;

namespace jjm.one.RabbitMqClientWrapper.di.core
{
	/// <summary>
	/// This class contains function for dependency injection of the <see cref="RmqcCore"/> class.
	/// </summary>
	public static class RmqcCoreDi
    {
	    /// <summary>
	    /// Adds all dependencies ot the <see cref="RmqcCore"/> class to a <see cref="ServiceCollection"/>.
	    /// </summary>
	    /// <param name="services"></param>
	    /// <param name="settings"></param>
	    /// <param name="enableCoreLogging"></param>
	    /// <returns></returns>
		public static IServiceCollection AddRmqcCore(this IServiceCollection services, Settings settings,
		    DiSimpleTypeWrappersEnableCoreLogging enableCoreLogging)
		{
			services.AddSingleton(settings);
			services.AddSingleton(enableCoreLogging);
			services.AddScoped<IRmqcCore, RmqcCore>();

			return services;
		}
    }
}


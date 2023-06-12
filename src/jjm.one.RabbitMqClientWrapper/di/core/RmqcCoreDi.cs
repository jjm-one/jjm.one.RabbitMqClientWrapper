using jjm.one.RabbitMqClientWrapper.main.core;
using jjm.one.RabbitMqClientWrapper.types;
using Microsoft.Extensions.DependencyInjection;

namespace jjm.one.RabbitMqClientWrapper.di.core;

/// <summary>
/// This class contains function for dependency injection of the <see cref="RmqcCore"/> class.
/// </summary>
internal static class RmqcCoreDi
{
    /// <summary>
    /// Adds all dependencies ot the <see cref="RmqcCore"/> class to a <see cref="ServiceCollection"/>.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
	public static IServiceCollection AddRmqcCore(this IServiceCollection services, Settings settings)
	{
		services.AddSingleton(settings);
		services.AddScoped<IRmqcCore>(sp => ActivatorUtilities.CreateInstance<RmqcCore>(sp));

		return services;
	}
}


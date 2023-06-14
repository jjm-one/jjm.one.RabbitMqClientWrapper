using jjm.one.RabbitMqClientWrapper.di.core;
using jjm.one.RabbitMqClientWrapper.main;
using jjm.one.RabbitMqClientWrapper.types;
using Microsoft.Extensions.DependencyInjection;

namespace jjm.one.RabbitMqClientWrapper.di;

/// <summary>
/// This class contains function for dependency injection of the <see cref="RmqcWrapper"/> class.
/// </summary>
public static class RmqcWrapperDi
{
    /// <summary>
    /// Adds all dependencies ot the <see cref="RmqcWrapper"/> class to a <see cref="ServiceCollection"/>.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static IServiceCollection AddRmqcWrapper(this IServiceCollection services, RmqcSettings settings)
    {
        services.AddRmqcCore(settings);
        services.AddScoped<IRmqcWrapper>(sp => ActivatorUtilities.CreateInstance<RmqcWrapper>(sp));

        return services;
    }
}


using System;
using jjm.one.RabbitMqClientWrapper.di.core;
using jjm.one.RabbitMqClientWrapper.main;
using jjm.one.RabbitMqClientWrapper.main.core;
using jjm.one.RabbitMqClientWrapper.types;
using Microsoft.Extensions.DependencyInjection;

namespace jjm.one.RabbitMqClientWrapper.di
{
	public static class RMQCWrapperDi
	{
        public static IServiceCollection AddRMQCWrapper(this IServiceCollection services, Settings settings, bool enableWrapperLogging = false, bool enableCoreLogging = false)
        {
            services.AddRMQCCore(settings, enableCoreLogging);
            services.AddSingleton(new DiSimpleTypeWrappersEnableWrapperLogging(enableWrapperLogging));
            services.AddScoped<IRMQCWrapper, RMQCWrapper>();

            return services;
        }
    }
}


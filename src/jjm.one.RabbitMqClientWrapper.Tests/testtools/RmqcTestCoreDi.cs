using System;
using jjm.one.RabbitMqClientWrapper.main;
using jjm.one.RabbitMqClientWrapper.main.core;
using jjm.one.RabbitMqClientWrapper.types;
using jjm.one.RabbitMqClientWrapper.types.di;
using Microsoft.Extensions.DependencyInjection;

namespace jjm.one.RabbitMqClientWrapper.Tests.testtools
{
	public static class RmqcTestCoreDi
	{
        public static IServiceCollection AddRMQCTestCore(this IServiceCollection services,
            bool enableWrapperLogging = false,
            bool resultOnReturn = true, Exception? exceptionOnReturn = null,
            Message? messageOnReturn = null, uint? amountOnRetrun = null)
        {
            services.AddSingleton(new RmqcTestCoreArgs
            {
                ResultOnReturn = resultOnReturn,
                ExceptionOnReturn = exceptionOnReturn,
                MessageOnReturn = messageOnReturn,
                AmountOnReturn = amountOnRetrun
            }) ;
            services.AddScoped<IRmqcCore, RmqcTestCore>();

            services.AddSingleton(new DiSimpleTypeWrappersEnableWrapperLogging(enableWrapperLogging));
            services.AddScoped<IRmqcWrapper, RmqcWrapper>();

            return services;
        }
    }
}


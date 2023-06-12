using FluentAssertions;
using jjm.one.RabbitMqClientWrapper.di.core;
using jjm.one.RabbitMqClientWrapper.main.core;
using jjm.one.RabbitMqClientWrapper.types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace jjm.one.RabbitMqClientWrapper.Tests.di.core;

/// <summary>
/// This class contains the unit tests for the <see cref="RmqcCoreDi"/> class.
/// </summary>
public class RmqcCoreDiTests
{
    #region private members

    private readonly IHostBuilder _hostBuilder;

    #endregion
    
    #region ctor

    /// <summary>
    /// The default constructor of the <see cref="RmqcCoreDiTests"/> class.
    /// </summary>
    public RmqcCoreDiTests()
    {
        _hostBuilder = Host.CreateDefaultBuilder();
    }

    #endregion
    
    #region tests

    /// <summary>
    /// Tests the static AddRmqcCore function.
    /// </summary>
    [Fact]
    public void RmqcCoreDiTest_AddRmqcCoreTest()
    {
        // arrange + act
        _hostBuilder.ConfigureServices(services =>
            services.AddRmqcCore(new Settings()));
        var host = _hostBuilder.Build();

        host.Services.GetService<IRmqcCore>().Should().NotBeNull();
    }

    #endregion
}
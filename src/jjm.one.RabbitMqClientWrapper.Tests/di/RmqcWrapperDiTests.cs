using jjm.one.RabbitMqClientWrapper.di;
using jjm.one.RabbitMqClientWrapper.main;
using jjm.one.RabbitMqClientWrapper.types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace jjm.one.RabbitMqClientWrapper.Tests.di;

/// <summary>
///     This class contains the unit tests for the <see cref="RmqcWrapperDi" /> class.
/// </summary>
public class RmqcWrapperDiTests
{
    #region private members

    private readonly IHostBuilder _hostBuilder;

    #endregion

    #region ctor

    /// <summary>
    ///     The default constructor of the <see cref="RmqcWrapperDiTests" /> class.
    /// </summary>
    public RmqcWrapperDiTests()
    {
        _hostBuilder = Host.CreateDefaultBuilder();
    }

    #endregion

    #region tests

    /// <summary>
    ///     Tests the static AddRmqcCore function.
    /// </summary>
    [Fact]
    public void RmqcWrapperDiTest_AddRmqcWrapperTest()
    {
        // arrange + act
        _hostBuilder.ConfigureServices(services =>
            services.AddRmqcWrapper(new RmqcSettings()));
        var host = _hostBuilder.Build();

        // assert
        host.Services.GetService<RmqcSettings>().Should().NotBeNull();
        host.Services.GetService<IRmqcWrapper>().Should().NotBeNull();
    }

    #endregion
}
using jjm.one.RabbitMqClientWrapper.di;
using jjm.one.RabbitMqClientWrapper.main;
using jjm.one.RabbitMqClientWrapper.types;
using Microsoft.Extensions.DependencyInjection;

namespace jjm.one.RabbitMqClientWrapper.Tests.di;

/// <summary>
///     This class contains the unit tests for the <see cref="RmqcWrapperDi" /> class.
/// </summary>
public class RmqcWrapperDiTests
{
    #region tests

    /// <summary>
    /// This test checks if the AddRmqcWrapper method correctly adds the IRmqcWrapper service to the service collection.
    /// </summary>
    [Fact]
    public void AddRmqcWrapper_ShouldAddServicesCorrectly()
    {
        // Arrange
        var services = new ServiceCollection();
        var settings = new RmqcSettings();

        // Act
        services.AddRmqcWrapper(settings);
        var builtServices = services.BuildServiceProvider();

        // Assert
        var scopedService = builtServices.GetService<IRmqcWrapper>();
        scopedService.Should().NotBeNull("because IRmqcWrapper should be registered as a scoped service");

        // Ensure that a new instance is created for each scope
        using var scope = builtServices.CreateScope();
        var serviceInNewScope = scope.ServiceProvider.GetService<IRmqcWrapper>();
        serviceInNewScope.Should().NotBeSameAs(scopedService, "because a new instance should be created for each scope");
    }

    #endregion
}
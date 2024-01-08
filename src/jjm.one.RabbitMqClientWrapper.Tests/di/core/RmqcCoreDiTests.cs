using jjm.one.RabbitMqClientWrapper.di.core;
using jjm.one.RabbitMqClientWrapper.main.core;
using jjm.one.RabbitMqClientWrapper.types;
using Microsoft.Extensions.DependencyInjection;

namespace jjm.one.RabbitMqClientWrapper.Tests.di.core;

/// <summary>
///     This class contains the unit tests for the <see cref="RmqcCoreDi" /> class.
/// </summary>
public class RmqcCoreDiTests
{
    #region tests

    /// <summary>
    /// This test checks if the AddRmqcCore method correctly adds the RmqcSettings and IRmqcCore services to the service collection.
    /// </summary>
    [Fact]
    public void AddRmqcCore_ShouldAddServicesCorrectly()
    {
        // Arrange
        var services = new ServiceCollection();
        var settings = new RmqcSettings();

        // Act
        services.AddRmqcCore(settings);
        var builtServices = services.BuildServiceProvider();

        // Assert
        var singletonService = builtServices.GetService<RmqcSettings>();
        singletonService.Should().BeEquivalentTo(settings, "because the settings should be registered as a singleton");

        var scopedService = builtServices.GetService<IRmqcCore>();
        scopedService.Should().NotBeNull("because IRmqcCore should be registered as a scoped service");

        // Ensure that a new instance is created for each scope
        using var scope = builtServices.CreateScope();
        var serviceInNewScope = scope.ServiceProvider.GetService<IRmqcCore>();
        serviceInNewScope.Should().NotBeSameAs(scopedService, "because a new instance should be created for each scope");
    }

    #endregion
}
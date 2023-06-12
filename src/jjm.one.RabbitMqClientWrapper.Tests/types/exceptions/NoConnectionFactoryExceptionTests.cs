using FluentAssertions;
using jjm.one.RabbitMqClientWrapper.main.core;
using jjm.one.RabbitMqClientWrapper.types.exceptions;
using RabbitMQ.Client;

namespace jjm.one.RabbitMqClientWrapper.Tests.types.exceptions;

/// <summary>
/// This class contains the unit tests for the <see cref="NoConnectionFactoryException"/> class.
/// </summary>
public class NoConnectionFactoryExceptionTests
{
    #region tests

    #region ctor tests
    
    /// <summary>
    /// Tests the default constructor of the <see cref="NoConnectionFactoryException"/> class.
    /// </summary>
    [Fact]
    public void NoConnectionFactoryExceptionTest_DefaultCtorTest()
    {
        // arrange + act
        var e = new NoConnectionFactoryException();
        
        // assert
        e.Message.Should().Be($"The {nameof(IConnectionFactory)} is null! " +
                              $"Maybe the {nameof(RmqcCore)} was not initialized properly.");
    }
    
    #endregion

    #endregion
}
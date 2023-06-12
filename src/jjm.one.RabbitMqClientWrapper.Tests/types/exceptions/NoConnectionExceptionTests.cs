using FluentAssertions;
using jjm.one.RabbitMqClientWrapper.main.core;
using jjm.one.RabbitMqClientWrapper.types.exceptions;
using RabbitMQ.Client;

namespace jjm.one.RabbitMqClientWrapper.Tests.types.exceptions;

/// <summary>
/// This class contains the unit tests for the <see cref="NoConnectionException"/> class.
/// </summary>
public class NoConnectionExceptionTests
{
    #region tests

    #region ctor tests
    
    /// <summary>
    /// Tests the default constructor of the <see cref="NoConnectionException"/> class.
    /// </summary>
    [Fact]
    public void NoConnectionExceptionTest_DefaultCtorTest()
    {
        // arrange + act
        var e = new NoConnectionException();
        
        // assert
        e.Message.Should().Be($"The {nameof(IConnection)} is null! " +
                              $"Maybe the {nameof(RmqcCore)} was not initialized properly.");
    }
    
    #endregion

    #endregion
}
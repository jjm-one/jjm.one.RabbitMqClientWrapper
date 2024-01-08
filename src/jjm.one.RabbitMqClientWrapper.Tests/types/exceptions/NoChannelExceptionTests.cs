using jjm.one.RabbitMqClientWrapper.main.core;
using jjm.one.RabbitMqClientWrapper.types.exceptions;
using RabbitMQ.Client;

namespace jjm.one.RabbitMqClientWrapper.Tests.types.exceptions;

/// <summary>
///     This class contains the unit tests for the <see cref="NoChannelException" /> class.
/// </summary>
public class NoChannelExceptionTests
{
    #region tests

    #region ctor tests

    /// <summary>
    ///     Tests the default constructor of the <see cref="NoChannelException" /> class.
    /// </summary>
    [Fact]
    public void NoChannelExceptionTest_DefaultCtorTest()
    {
        // arrange + act
        var e = new NoChannelException();

        // assert
        e.Message.Should().Be($"The {nameof(IModel)} is null! " +
                              $"Maybe the {nameof(RmqcCore)} was not initialized properly.");
    }

    #endregion

    #endregion
}
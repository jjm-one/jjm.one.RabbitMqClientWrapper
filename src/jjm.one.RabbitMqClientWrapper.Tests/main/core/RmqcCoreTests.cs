using FluentAssertions;
using jjm.one.RabbitMqClientWrapper.main.core;
using jjm.one.RabbitMqClientWrapper.types;
using Microsoft.Extensions.Logging;
using Moq;
using RabbitMQ.Client;

namespace jjm.one.RabbitMqClientWrapper.Tests.main.core;

/// <summary>
/// This class contains the unit tests for the <see cref="RmqcCore"/> class.
/// </summary>
public class RmqcCoreTests
{
    #region private members

    private readonly RmqcCore _sut1;
    private readonly Mock<RmqcCore> _sut2;
    private readonly Mock<IConnectionFactory> _connectionFactoryMock;
    private readonly Mock<IConnection> _connectionMock;
    private readonly Mock<IModel> _channelMock;
    private readonly Mock<ILogger<RmqcCore>> _rmqcCoreLoggingMock;

    #endregion

    #region ctors

    public RmqcCoreTests()
    {
        _sut2 = new Mock<RmqcCore>();
        _connectionFactoryMock = new Mock<IConnectionFactory>();
        _connectionMock = new Mock<IConnection>();
        _channelMock = new Mock<IModel>();
        _rmqcCoreLoggingMock = new Mock<ILogger<RmqcCore>>();

        _sut1 = new RmqcCore(new Settings(), _rmqcCoreLoggingMock.Object,
            _connectionFactoryMock.Object, _connectionMock.Object, _channelMock.Object);
    }

    #endregion

    #region tests

    #region ctor tests

    /// <summary>
    /// Tests the constructor of the <see cref="RmqcCore"/> class.
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_CtorTest()
    {
        // arrange + act
        var res = new RmqcCore(new Settings("Test"));
        
        // assert
        res.Settings.Hostname.Should().Be("Test");
    }

    #endregion
    
    

    #endregion
}
using System;
using jjm.one.RabbitMqClientWrapper.main.core;
using jjm.one.RabbitMqClientWrapper.types;
using jjm.one.RabbitMqClientWrapper.types.events;
using jjm.one.RabbitMqClientWrapper.types.exceptions;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace jjm.one.RabbitMqClientWrapper.Tests.main.core;

/// <summary>
///     This class contains the unit tests for the <see cref="RmqcCore" /> class.
/// </summary>
public class RmqcCoreTests
{
    #region ctors

    /// <summary>
    ///     The default constructor of the <see cref="RmqcCoreTests" /> class.
    /// </summary>
    public RmqcCoreTests()
    {
        _connectionFactoryMock = new Mock<IConnectionFactory>();
        _connectionMock = new Mock<IConnection>();
        _channelMock = new Mock<IModel>();
        _rmqcCoreLoggingMock = new Mock<ILogger<RmqcCore>>();
        _settingsMock = new Mock<RmqcSettings>();

        _sut = new RmqcCore(_settingsMock.Object, _rmqcCoreLoggingMock.Object,
            _connectionFactoryMock.Object, _connectionMock.Object, _channelMock.Object);
    }

    #endregion

    #region private members

    private RmqcCore _sut;
    private readonly Mock<IConnectionFactory> _connectionFactoryMock;
    private readonly Mock<IConnection> _connectionMock;
    private readonly Mock<IModel> _channelMock;
    private readonly Mock<ILogger<RmqcCore>> _rmqcCoreLoggingMock;
    private readonly Mock<RmqcSettings> _settingsMock;

    #endregion

    #region tests

    #region ctor tests

    /// <summary>
    ///     Tests the constructor of the <see cref="RmqcCore" /> class. (Test 1)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_CtorTest1()
    {
        // arrange

        // act
        _sut = new RmqcCore(_settingsMock.Object);

        // assert
        _sut.Should().NotBeNull();
        _sut.Settings.Should().NotBeNull();
    }

    /// <summary>
    ///     Tests the constructor of the <see cref="RmqcCore" /> class. (Test 2)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_CtorTest2()
    {
        // arrange

        // act
        _sut = new RmqcCore(_settingsMock.Object, _rmqcCoreLoggingMock.Object);

        // assert
        _sut.Should().NotBeNull();
        _sut.Settings.Should().NotBeNull();
    }

    #endregion

    #region public members tests

    /// <summary>
    ///     Tests the getter of the Settings member. (Test 1)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_SettingsGetTest1()
    {
        // arrange
        _settingsMock.SetupGet(x => x.Hostname).Returns("Test");
        _settingsMock.SetupGet(x => x.Port).Returns(42);
        _settingsMock.SetupGet(x => x.Username).Returns("Test");
        _settingsMock.SetupGet(x => x.Password).Returns("Test");
        _settingsMock.SetupGet(x => x.VirtualHost).Returns("Test");
        _settingsMock.SetupGet(x => x.Exchange).Returns("Test");
        _settingsMock.SetupGet(x => x.Queue).Returns("Test");

        // act
        _sut = new RmqcCore(_settingsMock.Object);

        // assert
        _sut.Settings.Should().NotBeNull();
        _sut.Settings.Hostname.Should().Be("Test");
        _sut.Settings.Port.Should().Be(42);
        _sut.Settings.Username.Should().Be("Test");
        _sut.Settings.Password.Should().Be("Test");
        _sut.Settings.VirtualHost.Should().Be("Test");
        _sut.Settings.Exchange.Should().Be("Test");
        _sut.Settings.Queue.Should().Be("Test");
    }

    /// <summary>
    ///     Tests the setter of the Settings member. (Test 1)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_SettingsSetTest1()
    {
        // arrange
        var settingsMockTemp = new Mock<RmqcSettings>();
        settingsMockTemp.SetupGet(x => x.Hostname).Returns("Test2");
        settingsMockTemp.SetupGet(x => x.Port).Returns(69);
        settingsMockTemp.SetupGet(x => x.Username).Returns("Test2");
        settingsMockTemp.SetupGet(x => x.Password).Returns("Test2");
        settingsMockTemp.SetupGet(x => x.VirtualHost).Returns("Test2");
        settingsMockTemp.SetupGet(x => x.Exchange).Returns("Test2");
        settingsMockTemp.SetupGet(x => x.Queue).Returns("Test2");


        // act
        _sut.Settings = settingsMockTemp.Object;

        // assert
        _sut.Settings.Should().NotBeNull();
        _sut.Settings.Hostname.Should().Be("Test2");
        _sut.Settings.Port.Should().Be(69);
        _sut.Settings.Username.Should().Be("Test2");
        _sut.Settings.Password.Should().Be("Test2");
        _sut.Settings.VirtualHost.Should().Be("Test2");
        _sut.Settings.Exchange.Should().Be("Test2");
        _sut.Settings.Queue.Should().Be("Test2");
    }

    /// <summary>
    ///     Tests the setter of the Settings member. (Test 2)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_SettingsSetTest2()
    {
        // arrange
        var settingsMockTemp = new Mock<RmqcSettings>();
        settingsMockTemp.Setup(x => x.Hostname).Returns("test");
        settingsMockTemp.Setup(x => x.Username).Returns("TestUser");
        settingsMockTemp.Setup(x => x.Password).Returns("TestPwd");
        _channelMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.Close());
        _channelMock.Setup(x => x.Dispose());
        _connectionMock.Setup(x => x.IsOpen).Returns(true);
        _connectionMock.Setup(x => x.Close());
        _connectionMock.Setup(x => x.Dispose());

        // act
        _sut.Settings = settingsMockTemp.Object;

        // assert
        _sut.Settings.Should().NotBeNull();
        _channelMock.Verify(x => x.IsOpen, Times.Once);
        _channelMock.Verify(x => x.Close(), Times.Once);
        _channelMock.Verify(x => x.Dispose(), Times.Once);
        _connectionMock.Verify(x => x.IsOpen, Times.Once);
        _connectionMock.Verify(x => x.Close(), Times.Once);
        _connectionMock.Verify(x => x.Dispose(), Times.Once);
    }

    /// <summary>
    ///     Tests the getter of the Connected member. (Test 1)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_ConnectedGetTest1()
    {
        // arrange
        _connectionMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.IsOpen).Returns(true);

        // act
        var b = _sut.Connected;

        // assert
        _connectionMock.Verify(x => x.IsOpen, Times.Once);
        _channelMock.Verify(x => x.IsOpen, Times.Once);
        b.Should().BeTrue();
    }

    /// <summary>
    ///     Tests the getter of the Connected member. (Test 2)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_ConnectedGetTest2()
    {
        // arrange
        _connectionMock.Setup(x => x.IsOpen).Returns(false);
        _channelMock.Setup(x => x.IsOpen).Returns(true);

        // act
        var b = _sut.Connected;

        // assert
        _connectionMock.Verify(x => x.IsOpen, Times.Once);
        _channelMock.Verify(x => x.IsOpen, Times.Never);
        b.Should().BeFalse();
    }

    /// <summary>
    ///     Tests the getter of the Connected member. (Test 3)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_ConnectedGetTest3()
    {
        // arrange
        _connectionMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.IsOpen).Returns(false);

        // act
        var b = _sut.Connected;

        // assert
        _connectionMock.Verify(x => x.IsOpen, Times.Once);
        _channelMock.Verify(x => x.IsOpen, Times.Once);
        b.Should().BeFalse();
    }

    /// <summary>
    ///     Tests the getter of the Connected member. (Test 4)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_ConnectedGetTest4()
    {
        // arrange
        _sut = new RmqcCore(_settingsMock.Object, _rmqcCoreLoggingMock.Object,
            null, _connectionMock.Object, _channelMock.Object);
        _connectionMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.IsOpen).Returns(true);

        // act
        var b = _sut.Connected;

        // assert
        _connectionMock.Verify(x => x.IsOpen, Times.Never);
        _channelMock.Verify(x => x.IsOpen, Times.Never);
        b.Should().BeFalse();
    }

    /// <summary>
    ///     Tests the getter of the Connected member. (Test 5)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_ConnectedGetTest5()
    {
        // arrange
        _sut = new RmqcCore(_settingsMock.Object, _rmqcCoreLoggingMock.Object,
            _connectionFactoryMock.Object, null, _channelMock.Object);
        _connectionMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.IsOpen).Returns(true);

        // act
        var b = _sut.Connected;

        // assert
        _connectionMock.Verify(x => x.IsOpen, Times.Never);
        _channelMock.Verify(x => x.IsOpen, Times.Never);
        b.Should().BeFalse();
    }

    /// <summary>
    ///     Tests the getter of the Connected member. (Test 6)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_ConnectedGetTest6()
    {
        // arrange
        _sut = new RmqcCore(_settingsMock.Object, _rmqcCoreLoggingMock.Object,
            _connectionFactoryMock.Object, _connectionMock.Object, null);
        _connectionMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.IsOpen).Returns(true);

        // act
        var b = _sut.Connected;

        // assert
        _connectionMock.Verify(x => x.IsOpen, Times.Once);
        _channelMock.Verify(x => x.IsOpen, Times.Never);
        b.Should().BeFalse();
    }

    #endregion

    #region public methods tests

    #region init & de-init

    /// <summary>
    ///     Testes the Init method.
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_InitTest()
    {
        // arrange
        _settingsMock.Setup(x => x.Hostname).Returns("test");
        _settingsMock.Setup(x => x.Username).Returns("TestUser");
        _settingsMock.Setup(x => x.Password).Returns("TestPwd");

        try
        {
            // act
            _sut.Init();
        }
        catch (Exception exc)
        {
            // assert
            Assert.Fail(exc.Message);
        }
    }

    /// <summary>
    ///     Testes the DeInit method.
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_DeInitTest()
    {
        // arrange

        try
        {
            // act
            _sut.DeInit();
        }
        catch (Exception exc)
        {
            // assert
            Assert.Fail(exc.Message);
        }
    }

    #endregion

    #region Connect

    /// <summary>
    ///     Testes the Connect method. (Test 1)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_ConnectTest1()
    {
        // arrange
        _connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(_connectionMock.Object);
        _connectionMock.Setup(x => x.CreateModel()).Returns(_channelMock.Object);

        // act
        var res = _sut.Connect(out var resExc);

        // assert
        _connectionFactoryMock.Verify(x => x.CreateConnection(), Times.Once);
        _connectionMock.Verify(x => x.CreateModel(), Times.Once);
        res.Should().BeTrue();
        resExc.Should().BeNull();
    }

    /// <summary>
    ///     Testes the Connect method. (Test 2)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_ConnectTest2()
    {
        // arrange
        _connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(value: null!);
        _connectionMock.Setup(x => x.CreateModel()).Returns(_channelMock.Object);

        // act
        var res = _sut.Connect(out var resExc);

        // assert
        _connectionFactoryMock.Verify(x => x.CreateConnection(), Times.Once);
        _connectionMock.Verify(x => x.CreateModel(), Times.Never);
        res.Should().BeFalse();
        resExc.Should().BeOfType<NoConnectionException>();
    }

    /// <summary>
    ///     Testes the Connect method. (Test 3)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_ConnectTest3()
    {
        // arrange
        _connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(_connectionMock.Object);
        _connectionMock.Setup(x => x.CreateModel()).Returns(value: null!);

        // act
        var res = _sut.Connect(out var resExc);

        // assert
        _connectionFactoryMock.Verify(x => x.CreateConnection(), Times.Once);
        _connectionMock.Verify(x => x.CreateModel(), Times.Once);
        res.Should().BeFalse();
        resExc.Should().BeOfType<NoChannelException>();
    }

    /// <summary>
    ///     Testes the Connect method. (Test 4)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_ConnectTest4()
    {
        // arrange
        _sut = new RmqcCore(new RmqcSettings(), _rmqcCoreLoggingMock.Object,
            null, _connectionMock.Object, _channelMock.Object);
        _connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(_connectionMock.Object);
        _connectionMock.Setup(x => x.CreateModel()).Returns(_channelMock.Object);

        // act
        var res = _sut.Connect(out var resExc);

        // assert
        _connectionFactoryMock.Verify(x => x.CreateConnection(), Times.Never);
        _connectionMock.Verify(x => x.CreateModel(), Times.Never);
        res.Should().BeFalse();
        resExc.Should().BeOfType<NoConnectionFactoryException>();
    }

    /// <summary>
    ///     Testes the Connect method. (Test 5)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_ConnectTest5()
    {
        // arrange
        _connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(_connectionMock.Object);
        _connectionMock.Setup(x => x.CreateModel()).Returns(_channelMock.Object);

        // act
        var evt = Assert.RaisesAny<ConnectCompletedEventArgs>(
            x => _sut.ConnectCompleted += x,
            x => _sut.ConnectCompleted -= x,
            () => _sut.Connect(out _));

        // assert
        evt.Should().NotBeNull();
        evt.Sender.Should().Be(_sut);
        evt.Arguments.Successful.Should().BeTrue();
        evt.Arguments.Exception.Should().BeNull();
        evt.Arguments.CompletionTime.Should().HaveValue();
    }

    /// <summary>
    ///     Testes the Connect method. (Test 6)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_ConnectTest6()
    {
        // arrange
        _connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(value: null!);
        _connectionMock.Setup(x => x.CreateModel()).Returns(_channelMock.Object);

        // act
        var evt = Assert.RaisesAny<ConnectCompletedEventArgs>(
            x => _sut.ConnectCompleted += x,
            x => _sut.ConnectCompleted -= x,
            () => _sut.Connect(out _));

        // assert
        evt.Should().NotBeNull();
        evt.Sender.Should().Be(_sut);
        evt.Arguments.Successful.Should().BeFalse();
        evt.Arguments.Exception.Should().BeOfType<NoConnectionException>();
        evt.Arguments.CompletionTime.Should().HaveValue();
    }

    /// <summary>
    ///     Testes the Connect method. (Test 7)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_ConnectTest7()
    {
        // arrange
        _connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(_connectionMock.Object);
        _connectionMock.Setup(x => x.CreateModel()).Returns(value: null!);

        // act
        var evt = Assert.RaisesAny<ConnectCompletedEventArgs>(
            x => _sut.ConnectCompleted += x,
            x => _sut.ConnectCompleted -= x,
            () => _sut.Connect(out _));

        // assert
        evt.Should().NotBeNull();
        evt.Sender.Should().Be(_sut);
        evt.Arguments.Successful.Should().BeFalse();
        evt.Arguments.Exception.Should().BeOfType<NoChannelException>();
        evt.Arguments.CompletionTime.Should().HaveValue();
    }

    /// <summary>
    ///     Testes the Connect method. (Test 8)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_ConnectTest8()
    {
        // arrange
        _sut = new RmqcCore(new RmqcSettings(), _rmqcCoreLoggingMock.Object,
            null, _connectionMock.Object, _channelMock.Object);
        _connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(_connectionMock.Object);
        _connectionMock.Setup(x => x.CreateModel()).Returns(_channelMock.Object);

        // act
        var evt = Assert.RaisesAny<ConnectCompletedEventArgs>(
            x => _sut.ConnectCompleted += x,
            x => _sut.ConnectCompleted -= x,
            () => _sut.Connect(out _));

        // assert
        evt.Should().NotBeNull();
        evt.Sender.Should().Be(_sut);
        evt.Arguments.Successful.Should().BeFalse();
        evt.Arguments.Exception.Should().BeOfType<NoConnectionFactoryException>();
        evt.Arguments.CompletionTime.Should().HaveValue();
    }

    /// <summary>
    ///     Testes the Connect method. (Test 9)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_ConnectTest9()
    {
        // arrange
        _connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(_connectionMock.Object);
        _connectionMock.Setup(x => x.CreateModel()).Returns(_channelMock.Object);

        // act
        var evt = Assert.RaisesAny<ConnectionStatusChangedEventArgs>(
            x => _sut.ConnectionStateChanged += x,
            x => _sut.ConnectionStateChanged -= x,
            () => _sut.Connect(out _));

        // assert
        evt.Should().NotBeNull();
        evt.Sender.Should().Be(_sut);
        evt.Arguments.Status.Should().BeTrue();
    }

    /// <summary>
    ///     Testes the Connect method. (Test 10)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_ConnectTest10()
    {
        // arrange
        _connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(_connectionMock.Object);
        _connectionMock.Setup(x => x.CreateModel()).Returns(_channelMock.Object);
        _sut.Connect(out _);
        _connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(value: null!);
        _connectionMock.Setup(x => x.CreateModel()).Returns(_channelMock.Object);

        // act
        var evt = Assert.RaisesAny<ConnectionStatusChangedEventArgs>(
            x => _sut.ConnectionStateChanged += x,
            x => _sut.ConnectionStateChanged -= x,
            () => _sut.Connect(out _));

        // assert
        evt.Should().NotBeNull();
        evt.Sender.Should().Be(_sut);
        evt.Arguments.Status.Should().BeFalse();
    }

    /// <summary>
    ///     Testes the Connect method. (Test 11)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_ConnectTest11()
    {
        // arrange
        _connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(_connectionMock.Object);
        _connectionMock.Setup(x => x.CreateModel()).Returns(_channelMock.Object);
        _sut.Connect(out _);
        _connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(_connectionMock.Object);
        _connectionMock.Setup(x => x.CreateModel()).Returns(value: null!);

        // act
        var evt = Assert.RaisesAny<ConnectionStatusChangedEventArgs>(
            x => _sut.ConnectionStateChanged += x,
            x => _sut.ConnectionStateChanged -= x,
            () => _sut.Connect(out _));

        // assert
        evt.Should().NotBeNull();
        evt.Sender.Should().Be(_sut);
        evt.Arguments.Status.Should().BeFalse();
    }

    #endregion

    #region Disconnect

    /// <summary>
    ///     Testes the Disconnect method. (Test 1)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_DisconnectTest1()
    {
        // arrange
        _channelMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.Close());
        _channelMock.Setup(x => x.Dispose());
        _connectionMock.Setup(x => x.IsOpen).Returns(true);
        _connectionMock.Setup(x => x.Close());
        _connectionMock.Setup(x => x.Dispose());

        // act
        var res = _sut.Disconnect(out var resExc);

        // assert
        res.Should().BeTrue();
        resExc.Should().BeNull();
        _channelMock.Verify(x => x.IsOpen, Times.Once);
        _channelMock.Verify(x => x.Close(), Times.Once);
        _channelMock.Verify(x => x.Dispose(), Times.Once);
        _connectionMock.Verify(x => x.IsOpen, Times.Once);
        _connectionMock.Verify(x => x.Close(), Times.Once);
        _connectionMock.Verify(x => x.Dispose(), Times.Once);
    }

    /// <summary>
    ///     Testes the Disconnect method. (Test 2)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_DisconnectTest2()
    {
        // arrange
        _channelMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.Close()).Throws<Exception>();
        _channelMock.Setup(x => x.Dispose());
        _connectionMock.Setup(x => x.IsOpen).Returns(true);
        _connectionMock.Setup(x => x.Close());
        _connectionMock.Setup(x => x.Dispose());

        // act
        var res = _sut.Disconnect(out var resExc);

        // assert
        res.Should().BeFalse();
        resExc.Should().NotBeNull();
        _channelMock.Verify(x => x.IsOpen, Times.Once);
        _channelMock.Verify(x => x.Close(), Times.Once);
        _channelMock.Verify(x => x.Dispose(), Times.Never);
        _connectionMock.Verify(x => x.IsOpen, Times.Never);
        _connectionMock.Verify(x => x.Close(), Times.Never);
        _connectionMock.Verify(x => x.Dispose(), Times.Never);
    }

    /// <summary>
    ///     Testes the Disconnect method. (Test 3)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_DisconnectTest3()
    {
        // arrange
        _channelMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.Close());
        _channelMock.Setup(x => x.Dispose());
        _connectionMock.Setup(x => x.IsOpen).Returns(true);
        _connectionMock.Setup(x => x.Close()).Throws<Exception>();
        _connectionMock.Setup(x => x.Dispose());

        // act
        var res = _sut.Disconnect(out var resExc);

        // assert
        res.Should().BeFalse();
        resExc.Should().NotBeNull();
        _channelMock.Verify(x => x.IsOpen, Times.Once);
        _channelMock.Verify(x => x.Close(), Times.Once);
        _channelMock.Verify(x => x.Dispose(), Times.Once);
        _connectionMock.Verify(x => x.IsOpen, Times.Once);
        _connectionMock.Verify(x => x.Close(), Times.Once);
        _connectionMock.Verify(x => x.Dispose(), Times.Never);
    }

    /// <summary>
    ///     Testes the Disconnect method. (Test 4)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_DisconnectTest4()
    {
        // arrange
        _channelMock.Setup(x => x.IsOpen).Returns(false);
        _channelMock.Setup(x => x.Close());
        _channelMock.Setup(x => x.Dispose());
        _connectionMock.Setup(x => x.IsOpen).Returns(true);
        _connectionMock.Setup(x => x.Close());
        _connectionMock.Setup(x => x.Dispose());

        // act
        var res = _sut.Disconnect(out var resExc);

        // assert
        res.Should().BeTrue();
        resExc.Should().BeNull();
        _channelMock.Verify(x => x.IsOpen, Times.Once);
        _channelMock.Verify(x => x.Close(), Times.Never);
        _channelMock.Verify(x => x.Dispose(), Times.Once);
        _connectionMock.Verify(x => x.IsOpen, Times.Once);
        _connectionMock.Verify(x => x.Close(), Times.Once);
        _connectionMock.Verify(x => x.Dispose(), Times.Once);
    }

    /// <summary>
    ///     Testes the Disconnect method. (Test 5)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_DisconnectTest5()
    {
        // arrange
        _channelMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.Close());
        _channelMock.Setup(x => x.Dispose());
        _connectionMock.Setup(x => x.IsOpen).Returns(false);
        _connectionMock.Setup(x => x.Close());
        _connectionMock.Setup(x => x.Dispose());

        // act
        var res = _sut.Disconnect(out var resExc);

        // assert
        res.Should().BeTrue();
        resExc.Should().BeNull();
        _channelMock.Verify(x => x.IsOpen, Times.Once);
        _channelMock.Verify(x => x.Close(), Times.Once);
        _channelMock.Verify(x => x.Dispose(), Times.Once);
        _connectionMock.Verify(x => x.IsOpen, Times.Once);
        _connectionMock.Verify(x => x.Close(), Times.Never);
        _connectionMock.Verify(x => x.Dispose(), Times.Once);
    }

    /// <summary>
    ///     Testes the Disconnect method. (Test 6)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_DisconnectTest6()
    {
        // arrange
        _channelMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.Close());
        _channelMock.Setup(x => x.Dispose());
        _connectionMock.Setup(x => x.IsOpen).Returns(true);
        _connectionMock.Setup(x => x.Close());
        _connectionMock.Setup(x => x.Dispose());

        // act
        var evt = Assert.RaisesAny<DisconnectCompletedEventArgs>(
            x => _sut.DisconnectCompleted += x,
            x => _sut.DisconnectCompleted -= x,
            () => _sut.Disconnect(out _));

        // assert
        evt.Should().NotBeNull();
        evt.Sender.Should().Be(_sut);
        evt.Arguments.Successful.Should().BeTrue();
        evt.Arguments.Exception.Should().BeNull();
        evt.Arguments.CompletionTime.Should().HaveValue();
    }

    #endregion

    #region WriteMsg

    /// <summary>
    ///     Testes the WriteMsg method. (Test 1)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_WriteMsgTest1()
    {
        // arrange
        var m = new RmqcMessage();
        _connectionMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.BasicPublish(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
            It.IsAny<IBasicProperties>(), It.IsAny<ReadOnlyMemory<byte>>()));

        // act
        var res = _sut.WriteMsg(ref m, out var resExc);

        // assert
        _connectionMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.BasicPublish(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
            It.IsAny<IBasicProperties>(), It.IsAny<ReadOnlyMemory<byte>>()), Times.Once);
        res.Should().BeTrue();
        resExc.Should().BeNull();
    }

    /// <summary>
    ///     Testes the WriteMsg method. (Test 2)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_WriteMsgTest2()
    {
        // arrange
        var m = new RmqcMessage();
        _connectionMock.Setup(x => x.IsOpen).Returns(false);
        _channelMock.Setup(x => x.IsOpen).Returns(false);
        _channelMock.Setup(x => x.BasicPublish(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
            It.IsAny<IBasicProperties>(), It.IsAny<ReadOnlyMemory<byte>>()));

        // act
        var res = _sut.WriteMsg(ref m, out var resExc);

        // assert
        _connectionMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.BasicPublish(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
            It.IsAny<IBasicProperties>(), It.IsAny<ReadOnlyMemory<byte>>()), Times.Never);
        res.Should().BeFalse();
        resExc.Should().BeOfType<InvalidOperationException>();
    }

    #endregion

    #region ReadMsg

    /// <summary>
    ///     Testes the ReadMsg method. (Test 1)
    /// </summary>
    [Fact(Skip = "Not properly implemented!")]
    public void RmqcWrapperTest_ReadMsgTest1()
    {
        // arrange
        var bgr = new BasicGetResult(42, false, "TEST-EX", "TEST-RK", 69, null, null);
        var m = new RmqcMessage(bgr);
        _connectionMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.BasicGet(It.IsAny<string>(), It.IsAny<bool>())).Returns(bgr);

        // act
        var res = _sut.ReadMsg(out var resMsg, false, out var resExc);

        // assert
        _connectionMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.BasicGet(It.IsAny<string>(), It.IsAny<bool>()), Times.Once());
        res.Should().BeTrue();
        resMsg.Should().BeEquivalentTo(m);
        resExc.Should().BeNull();
    }

    /// <summary>
    ///     Testes the ReadMsg method. (Test 2)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_ReadMsgTest2()
    {
        // arrange
        var bgr = new BasicGetResult(42, false, "TEST-EX", "TEST-RK", 69, null, null);
        _connectionMock.Setup(x => x.IsOpen).Returns(false);
        _channelMock.Setup(x => x.IsOpen).Returns(false);
        _channelMock.Setup(x => x.BasicGet(It.IsAny<string>(), It.IsAny<bool>())).Returns(bgr);

        // act
        var res = _sut.ReadMsg(out var resMsg, false, out var resExc);

        // assert
        _connectionMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.BasicGet(It.IsAny<string>(), It.IsAny<bool>()), Times.Never());
        res.Should().BeFalse();
        resMsg.Should().BeNull();
        resExc.Should().BeOfType<InvalidOperationException>();
    }

    /// <summary>
    ///     Testes the ReadMsg method. (Test 3)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_ReadMsgTest3()
    {
        // arrange
        _connectionMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.BasicGet(It.IsAny<string>(), It.IsAny<bool>())).Returns(value: null!);

        // act
        var res = _sut.ReadMsg(out var resMsg, false, out var resExc);

        // assert
        _connectionMock.Verify(x => x.IsOpen, Times.Once);
        _channelMock.Verify(x => x.IsOpen, Times.Once);
        _channelMock.Verify(x => x.BasicGet(It.IsAny<string>(), It.IsAny<bool>()), Times.Once());
        res.Should().BeFalse();
        resMsg.Should().BeNull();
        resExc.Should().BeNull();
    }

    #endregion

    #region AckMsg

    /// <summary>
    ///     Testes the AckMsg method. (Test 1)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_AckMsgTest1()
    {
        // arrange
        var m = new RmqcMessage();
        _connectionMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.BasicAck(It.IsAny<ulong>(), It.IsAny<bool>()));

        // act
        var res = _sut.AckMsg(ref m, out var resExc);

        // assert
        _connectionMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.BasicAck(It.IsAny<ulong>(), It.IsAny<bool>()), Times.Once());
        res.Should().BeTrue();
        resExc.Should().BeNull();
    }

    /// <summary>
    ///     Testes the AckMsg method. (Test 2)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_AckMsgTest2()
    {
        // arrange
        var m = new RmqcMessage();
        _connectionMock.Setup(x => x.IsOpen).Returns(false);
        _channelMock.Setup(x => x.IsOpen).Returns(false);
        _channelMock.Setup(x => x.BasicAck(It.IsAny<ulong>(), It.IsAny<bool>()));

        // act
        var res = _sut.AckMsg(ref m, out var resExc);

        // assert
        _connectionMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.BasicAck(It.IsAny<ulong>(), It.IsAny<bool>()), Times.Never());
        res.Should().BeFalse();
        resExc.Should().BeOfType<InvalidOperationException>();
    }

    #endregion

    #region NackMsg

    /// <summary>
    ///     Testes the NackMsg method. (Test 1)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_NackMsgTest1()
    {
        // arrange
        var m = new RmqcMessage();
        _connectionMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.BasicNack(It.IsAny<ulong>(), It.IsAny<bool>(), It.IsAny<bool>()));

        // act
        var res = _sut.NackMsg(ref m, false, out var resExc);

        // assert
        _connectionMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.BasicNack(It.IsAny<ulong>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Once());
        res.Should().BeTrue();
        resExc.Should().BeNull();
    }

    /// <summary>
    ///     Testes the NackMsg method. (Test 2)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_NackMsgTest2()
    {
        // arrange
        var m = new RmqcMessage();
        _connectionMock.Setup(x => x.IsOpen).Returns(false);
        _channelMock.Setup(x => x.IsOpen).Returns(false);
        _channelMock.Setup(x => x.BasicNack(It.IsAny<ulong>(), It.IsAny<bool>(), It.IsAny<bool>()));

        // act
        var res = _sut.NackMsg(ref m, false, out var resExc);

        // assert
        _connectionMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.BasicNack(It.IsAny<ulong>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Never());
        res.Should().BeFalse();
        resExc.Should().BeOfType<InvalidOperationException>();
    }

    #endregion

    #region WaitForWriteConfirm

    /// <summary>
    ///     Testes the WaitForWriteConfirm method. (Test 1)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_WaitForWriteConfirmTest1()
    {
        // arrange
        var t = new TimeSpan();
        _connectionMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.WaitForConfirms(It.IsAny<TimeSpan>())).Returns(true);

        // act
        var res = _sut.WaitForWriteConfirm(t, out var resExc);

        // assert
        _connectionMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.WaitForConfirms(It.IsAny<TimeSpan>()), Times.Once());
        res.Should().BeTrue();
        resExc.Should().BeNull();
    }

    /// <summary>
    ///     Testes the WaitForWriteConfirm method. (Test 2)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_WaitForWriteConfirmTest2()
    {
        // arrange
        var t = new TimeSpan();
        _connectionMock.Setup(x => x.IsOpen).Returns(false);
        _channelMock.Setup(x => x.IsOpen).Returns(false);
        _channelMock.Setup(x => x.WaitForConfirms(It.IsAny<TimeSpan>())).Returns(true);

        // act
        var res = _sut.WaitForWriteConfirm(t, out var resExc);

        // assert
        _connectionMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.WaitForConfirms(It.IsAny<TimeSpan>()), Times.Never());
        res.Should().BeFalse();
        resExc.Should().BeOfType<InvalidOperationException>();
    }

    #endregion

    #region QueuedMsgs

    /// <summary>
    ///     Testes the QueuedMsgs method. (Test 1)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_QueuedMsgsTest1()
    {
        // arrange
        _connectionMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.MessageCount(It.IsAny<string>())).Returns(42);

        // act
        var res = _sut.QueuedMsgs(out var a, out var resExc);

        // assert
        _connectionMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.MessageCount(It.IsAny<string>()), Times.Once());
        res.Should().BeTrue();
        a.Should().Be(42);
        resExc.Should().BeNull();
    }

    /// <summary>
    ///     Testes the QueuedMsgs method. (Test 2)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_QueuedMsgsTest2()
    {
        // arrange
        _connectionMock.Setup(x => x.IsOpen).Returns(false);
        _channelMock.Setup(x => x.IsOpen).Returns(false);
        _channelMock.Setup(x => x.MessageCount(It.IsAny<string>())).Returns(42);

        // act
        var res = _sut.QueuedMsgs(out var a, out var resExc);

        // assert
        _connectionMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.MessageCount(It.IsAny<string>()), Times.Never());
        res.Should().BeFalse();
        a.Should().BeNull();
        resExc.Should().BeOfType<InvalidOperationException>();
    }

    #endregion

    #endregion

    #endregion
}
using System;
using FluentAssertions;
using jjm.one.RabbitMqClientWrapper.main.core;
using jjm.one.RabbitMqClientWrapper.types;
using jjm.one.RabbitMqClientWrapper.types.exceptions;
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

    private RmqcCore _sut;
    private readonly Mock<IConnectionFactory> _connectionFactoryMock;
    private readonly Mock<IConnection> _connectionMock;
    private readonly Mock<IModel> _channelMock;
    private readonly Mock<ILogger<RmqcCore>> _rmqcCoreLoggingMock;

    #endregion

    #region ctors

    /// <summary>
    /// The default constructor of the <see cref="RmqcCoreTests"/> class.
    /// </summary>
    public RmqcCoreTests()
    {
        _connectionFactoryMock = new Mock<IConnectionFactory>();
        _connectionMock = new Mock<IConnection>();
        _channelMock = new Mock<IModel>();
        _rmqcCoreLoggingMock = new Mock<ILogger<RmqcCore>>();

        _sut = new RmqcCore(new Settings(), _rmqcCoreLoggingMock.Object,
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
    
    #region public members tests
    
    /// <summary>
    /// Tests the getter of the Settings member. (Test 1)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_SettingsGetTest1()
    {
        // arrange
        var sTest = new Settings();

        // act
        var s = _sut.Settings;

        // assert
        s.Should().Be(sTest);
    }
    
    /// <summary>
    /// Tests the getter of the Settings member. (Test 2)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_SettingsGetTest2()
    {
        // arrange
        var sTest = new Settings("Test");

        // act
        _sut.Settings = sTest;
        var s = _sut.Settings;

        // assert
        s.Should().Be(sTest);
    }
    
    /// <summary>
    /// Tests the setter of the Settings member. (Test 1)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_SettingsSetTest1()
    {
        // arrange
        var sTest = new Settings();

        // act
        _sut.Settings = sTest;

        // assert
        _channelMock.Verify(x => x.IsOpen, Times.Never);
        _channelMock.Verify(x => x.Close(), Times.Never);
        _channelMock.Verify(x => x.Dispose(), Times.Never);
        _connectionMock.Verify(x => x.IsOpen, Times.Never);
        _connectionMock.Verify(x => x.Close(), Times.Never);
        _connectionMock.Verify(x => x.Dispose(), Times.Never);

        _sut.Settings.Should().Be(sTest);
    }
    
    /// <summary>
    /// Tests the setter of the Settings member. (Test 2)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_SettingsSetTest2()
    {
        // arrange
        var sTest = new Settings("Test");
        _channelMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.Close());
        _channelMock.Setup(x => x.Dispose());
        _connectionMock.Setup(x => x.IsOpen).Returns(true);
        _connectionMock.Setup(x => x.Close());
        _connectionMock.Setup(x => x.Dispose());
        
        // act
        _sut.Settings = sTest;

        // assert
        _channelMock.Verify(x => x.IsOpen, Times.Once);
        _channelMock.Verify(x => x.Close(), Times.Once);
        _channelMock.Verify(x => x.Dispose(), Times.Once);
        _connectionMock.Verify(x => x.IsOpen, Times.Once);
        _connectionMock.Verify(x => x.Close(), Times.Once);
        _connectionMock.Verify(x => x.Dispose(), Times.Once);

        _sut.Settings.Should().Be(sTest);
    }
    
    /// <summary>
    /// Tests the getter of the Connected member. (Test 1)
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
    /// Tests the getter of the Connected member. (Test 2)
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
        _channelMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        b.Should().BeFalse();
    }
    
    /// <summary>
    /// Tests the getter of the Connected member. (Test 3)
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
        _connectionMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.IsOpen, Times.Once);
        b.Should().BeFalse();
    }

    /// <summary>
    /// Tests the getter of the Connected member. (Test 4)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_ConnectedGetTest4()
    {
        // arrange
        _sut = new RmqcCore(new Settings(), _rmqcCoreLoggingMock.Object,
            null, _connectionMock.Object, _channelMock.Object);
        _connectionMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.IsOpen).Returns(true);

        // act
        var b = _sut.Connected;

        // assert
        _connectionMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        b.Should().BeFalse();
    }

    
    #endregion
    
    #region public methods tests

    /// <summary>
    /// Testes the Init method.
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_InitTest()
    {
        // arrange

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
    /// Testes the DeInit method.
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

    /// <summary>
    /// Testes the Connect method. (Test 1)
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
    /// Testes the Connect method. (Test 2)
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
        _connectionMock.Verify(x => x.CreateModel(), Times.AtMostOnce);
        res.Should().BeFalse();
        resExc.Should().BeOfType<NoConnectionException>();
    }
    
    /// <summary>
    /// Testes the Connect method. (Test 3)
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
        _connectionFactoryMock.Verify(x => x.CreateConnection(), Times.AtMostOnce);
        _connectionMock.Verify(x => x.CreateModel(), Times.Once);
        res.Should().BeFalse();
        resExc.Should().BeOfType<NoChannelException>();
    }
    
    /// <summary>
    /// Testes the Connect method. (Test 4)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_ConnectTest4()
    {
        // arrange
        _sut = new RmqcCore(new Settings(), _rmqcCoreLoggingMock.Object,
            null, _connectionMock.Object, _channelMock.Object);
        _connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(_connectionMock.Object);
        _connectionMock.Setup(x => x.CreateModel()).Returns(value: null!);

        // act
        var res = _sut.Connect(out var resExc);

        // assert
        _connectionFactoryMock.Verify(x => x.CreateConnection(), Times.AtMostOnce);
        _connectionMock.Verify(x => x.CreateModel(), Times.AtMostOnce);
        res.Should().BeFalse();
        resExc.Should().BeOfType<NoConnectionFactoryException>();
    }
    
    /// <summary>
    /// Testes the Disconnect method.
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_DisconnectTest()
    {
        // arrange
        _channelMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.Close());
        _channelMock.Setup(x => x.Dispose());
        _connectionMock.Setup(x => x.IsOpen).Returns(true);
        _connectionMock.Setup(x => x.Close());
        _connectionMock.Setup(x => x.Dispose());

        // act
        _sut.Disconnect();
            
        // assert
        _channelMock.Verify(x => x.IsOpen, Times.Once);
        _channelMock.Verify(x => x.Close(), Times.Once);
        _channelMock.Verify(x => x.Dispose(), Times.Once);
        _connectionMock.Verify(x => x.IsOpen, Times.Once);
        _connectionMock.Verify(x => x.Close(), Times.Once);
        _connectionMock.Verify(x => x.Dispose(), Times.Once);
    }

    /// <summary>
    /// Testes the WriteMsg method. 
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_WriteMsgTest()
    {
        // arrange
        var m = new Message();
        _connectionMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.BasicPublish(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
            It.IsAny<IBasicProperties>(), It.IsAny<ReadOnlyMemory<byte>>()));

        // act
        var res = _sut.WriteMsg(m, out var resExc); 
        
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
    /// Testes the ReadMsg method. 
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_ReadMsgTest()
    {
        // arrange
        var bgr = new BasicGetResult(42, false, "TEST-EX", "TEST-RK", 69, null, null);
        var m = new Message(bgr);
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
    /// Testes the AckMsg method. 
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_AckMsgTest()
    {
        // arrange
        var m = new Message();
        _connectionMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.BasicAck(It.IsAny<ulong>(), It.IsAny<bool>()));

        // act
        var res = _sut.AckMsg(m, out var resExc); 
        
        // assert
        _connectionMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.BasicAck(It.IsAny<ulong>(), It.IsAny<bool>()), Times.Once());
        res.Should().BeTrue();
        resExc.Should().BeNull();
    }

    /// <summary>
    /// Testes the NackMsg method. 
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_NackMsgTest()
    {
        // arrange
        var m = new Message();
        _connectionMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.BasicNack(It.IsAny<ulong>(), It.IsAny<bool>(),It.IsAny<bool>()));

        // act
        var res = _sut.NackMsg(m, false, out var resExc); 
        
        // assert
        _connectionMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.BasicNack(It.IsAny<ulong>(), It.IsAny<bool>(),It.IsAny<bool>()), Times.Once());
        res.Should().BeTrue();
        resExc.Should().BeNull();
    }

    /// <summary>
    /// Testes the WaitForWriteConfirm method. 
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_WaitForWriteConfirmTest()
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
    /// Testes the QueuedMsgs method. 
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_QueuedMsgsTest()
    {
        // arrange
        uint? a = 0;
        _connectionMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.IsOpen).Returns(true);
        _channelMock.Setup(x => x.MessageCount(It.IsAny<string>())).Returns(42);

        // act
        var res = _sut.QueuedMsgs(out a, out var resExc); 
        
        // assert
        _connectionMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.IsOpen, Times.AtMostOnce);
        _channelMock.Verify(x => x.MessageCount(It.IsAny<string>()), Times.Once());
        res.Should().BeTrue();
        a.Should().Be(42);
        resExc.Should().BeNull();
    }
    
    #endregion
    
    #endregion
}
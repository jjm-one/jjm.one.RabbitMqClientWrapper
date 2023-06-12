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

    private readonly RmqcCore _sut;
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

    #endregion
    
    #region public methods tests

    /// <summary>
    /// Testes the Init method.
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_InitTest()
    {
        // arrange

        // act
        _sut.Init();

        // assert
    }
    
    /// <summary>
    /// Testes the DeInit method.
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_DeInitTest()
    {
        // arrange

        // act
        _sut.DeInit();

        // assert
    }

    /// <summary>
    /// Testes the Connect method. (Test 1)
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_ConnectTest1()
    {
        // arrange
        Exception? e = null;
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
        Exception? e = null;
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
        Exception? e = null;
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
    
   /* 
    /// <summary>
    /// Testes the Disconnect method.
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_DisconnectTest()
    {
        // arrange
        _rmqcCoreMock.Setup(x => x.Disconnect());

        // act
        _sut.Disconnect();
            
        // assert
        _rmqcCoreMock.Verify(x => x.Disconnect(), Times.Once);
    }

    /// <summary>
    /// Testes the WriteMsg method. 
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_WriteMsgTest()
    {
        // arrange
        Exception? e;
        var m = new Message();
        _rmqcCoreMock.Setup(x => x.WriteMsg(m,out e)).Returns(true);

        // act
        var res = _sut.WriteMsg(m, out var resExc); 
        
        // assert
        res.Should().BeTrue();
        resExc.Should().BeNull();
        _rmqcCoreMock.Verify(x => 
            x.WriteMsg(It.IsAny<Message>(), out It.Ref<Exception?>.IsAny), Times.Once);
    }

    /// <summary>
    /// Testes the ReadMsg method. 
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_ReadMsgTest()
    {
        // arrange
        Exception? e;
        var m = new Message();
        _rmqcCoreMock.Setup(x => x.ReadMsg(out m, false, out e)).Returns(true);

        // act
        var res = _sut.ReadMsg(out m, false, out var resExc); 
        
        // assert
        res.Should().BeTrue();
        resExc.Should().BeNull();
        _rmqcCoreMock.Verify(x => 
            x.ReadMsg(out It.Ref<Message?>.IsAny, false, out It.Ref<Exception?>.IsAny), Times.Once);
    }

    /// <summary>
    /// Testes the AckMsg method. 
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_AckMsgTest()
    {
        // arrange
        Exception? e;
        var m = new Message();
        _rmqcCoreMock.Setup(x => x.AckMsg(m, out e)).Returns(true);

        // act
        var res = _sut.AckMsg(m, out var resExc); 
        
        // assert
        res.Should().BeTrue();
        resExc.Should().BeNull();
        _rmqcCoreMock.Verify(x => 
            x.AckMsg(It.IsAny<Message>(), out It.Ref<Exception?>.IsAny), Times.Once);
    }

    /// <summary>
    /// Testes the NackMsg method. 
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_NackMsgTest()
    {
        // arrange
        Exception? e;
        var m = new Message();
        _rmqcCoreMock.Setup(x => x.NackMsg(m, false, out e)).Returns(true);

        // act
        var res = _sut.NackMsg(m, false, out var resExc); 
        
        // assert
        res.Should().BeTrue();
        resExc.Should().BeNull();
        _rmqcCoreMock.Verify(x => 
            x.NackMsg(It.IsAny<Message>(), false, out It.Ref<Exception?>.IsAny), Times.Once);
    }

    /// <summary>
    /// Testes the WaitForWriteConfirm method. 
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_WaitForWriteConfirmTest()
    {
        // arrange
        Exception? e;
        var t = new TimeSpan();
        _rmqcCoreMock.Setup(x => x.WaitForWriteConfirm(t, out e)).Returns(true);

        // act
        var res = _sut.WaitForWriteConfirm(t, out var resExc); 
        
        // assert
        res.Should().BeTrue();
        resExc.Should().BeNull();
        _rmqcCoreMock.Verify(x => 
            x.WaitForWriteConfirm(It.IsAny<TimeSpan>(), out It.Ref<Exception?>.IsAny), Times.Once);
    }

    /// <summary>
    /// Testes the QueuedMsgs method. 
    /// </summary>
    [Fact]
    public void RmqcWrapperTest_QueuedMsgsTest()
    {
        // arrange
        Exception? e;
       uint? a = 0;
        _rmqcCoreMock.Setup(x => x.QueuedMsgs(out a, out e)).Returns(true);

        // act
        var res = _sut.QueuedMsgs(out a, out var resExc); 
        
        // assert
        res.Should().BeTrue();
        resExc.Should().BeNull();
        _rmqcCoreMock.Verify(x => 
            x.QueuedMsgs(out It.Ref<uint?>.IsAny, out It.Ref<Exception?>.IsAny), Times.Once);
    }
    */
    #endregion
    
    #endregion
}
using System;
using FluentAssertions;
using jjm.one.RabbitMqClientWrapper.main;
using jjm.one.RabbitMqClientWrapper.main.core;
using jjm.one.RabbitMqClientWrapper.types;
using jjm.one.RabbitMqClientWrapper.types.di;
using Microsoft.Extensions.Logging;
using Moq;

namespace jjm.one.RabbitMqClientWrapper.Tests.main
{
    /// <summary>
    /// This class contains the unit tests for the <see cref="RmqcWrapper"/> class.
    /// </summary>
    public class RmqcWrapperTests
    {
        #region private members

        private readonly RmqcWrapper _sut;
        private readonly Mock<IRmqcCore> _rmqcCoreMock;
        private readonly Mock<ILogger<RmqcWrapper>> _rmqcWrapperLoggingMock;

        #endregion
        /// <summary>
        /// The default constructor of the <see cref="RmqcWrapperTests"/> class.
        /// </summary>
        public RmqcWrapperTests()
        {
            _rmqcCoreMock = new Mock<IRmqcCore>();
            _rmqcWrapperLoggingMock = new Mock<ILogger<RmqcWrapper>>();
            _sut = new RmqcWrapper(_rmqcCoreMock.Object, _rmqcWrapperLoggingMock.Object, 
                new DiSimpleTypeWrappersEnableWrapperLogging());
        }

        #region tests

        #region public members tests

        /// <summary>
        /// Tests the getter of the Settings member.
        /// </summary>
        [Fact]
        public void RmqcWrapperTest_SettingsGetTest()
        {
            // arrange
            var sTest = new Settings("test");
            _rmqcCoreMock.Setup(x => x.Settings).Returns(sTest);
            Settings s = null!;

            try
            {
                // act
                s = _sut.Settings;
            }
            catch (Exception exc)
            {
                // assert 1
                Assert.Fail(exc.Message);
            }
            
            // assert 2
            _rmqcCoreMock.Verify(x => x.Settings, Times.Once);
            s.Should().Be(sTest);
        }

        /// <summary>
        /// Tests the setter of the Settings member.
        /// </summary>
        [Fact]
        public void RmqcWrapperTest_SettingsSetTest()
        {
            // arrange
            var sTest = new Settings("test");
            _rmqcCoreMock.SetupSet(x => x.Settings=It.IsAny<Settings>()).Verifiable();

            try
            {
                // act
                _sut.Settings = sTest;
            }
            catch (Exception exc)
            {
                // assert 1
                Assert.Fail(exc.Message);
            }
            
            // assert 2
            _rmqcCoreMock.VerifySet(x => x.Settings=It.IsAny<Settings>(), Times.Once);
        }
        
        /// <summary>
        /// Tests the getter of the Connected member.
        /// </summary>
        [Fact]
        public void RmqcWrapperTest_ConnectedGetTest()
        {
            // arrange
            _rmqcCoreMock.Setup(x => x.Connected).Returns(true);
            var b = false;

            try
            {
                // act
                b = _sut.Connected;
            }
            catch (Exception exc)
            {
                // assert 1
                Assert.Fail(exc.Message);
            }
            
            // assert 2
            _rmqcCoreMock.Verify(x => x.Connected, Times.Once);
            b.Should().BeTrue();
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
            _rmqcCoreMock.Setup(x => x.Init());

            try
            {
                // act
                _sut.Init();
            }
            catch (Exception exc)
            {
                // assert 1
                Assert.Fail(exc.Message);
            }
            
            // assert 2
            _rmqcCoreMock.Verify(x => x.Init(), Times.Once);
        }
        
        /// <summary>
        /// Testes the DeInit method.
        /// </summary>
        [Fact]
        public void RmqcWrapperTest_DeInitTest()
        {
            // arrange
            _rmqcCoreMock.Setup(x => x.DeInit());

            try
            {
                // act
                _sut.DeInit();
            }
            catch (Exception exc)
            {
                // assert 1
                Assert.Fail(exc.Message);
            }
            
            // asser 2
            _rmqcCoreMock.Verify(x => x.DeInit(), Times.Once);
        }
        
        /// <summary>
        /// Testes the Connect method. (Test 1)
        /// </summary>
        [Fact]
        public void RmqcWrapperTest_ConnectTest1()
        {
            // arrange
            Exception? e;
            _rmqcCoreMock.Setup(x => x.Connect(out e)).Returns(true);

            // act
            var res = _sut.Connect(); 
            
            // assert
            res.Should().BeTrue();
            _rmqcCoreMock.Verify(x => x.Connect(out It.Ref<Exception?>.IsAny), Times.Once);
        }
        
        /// <summary>
        /// Testes the Connect method. (Test 2)
        /// </summary>
        [Fact]
        public void RmqcWrapperTest_ConnectTest2()
        {
            // arrange
            Exception? e = null;
            _rmqcCoreMock.Setup(x => x.Connect(out e)).Returns(true);

            // act
            var res = _sut.Connect(out var resExc);

            // assert
            res.Should().BeTrue();
            resExc.Should().BeNull();
            _rmqcCoreMock.Verify(x => x.Connect(out It.Ref<Exception?>.IsAny), Times.Once);
        }
        
        /// <summary>
        /// Testes the Disconnect method.
        /// </summary>
        [Fact]
        public void RmqcWrapperTest_DisconnectTest()
        {
            // arrange
            _rmqcCoreMock.Setup(x => x.Disconnect());

            try
            {
                // act
                _sut.Disconnect();
            }
            catch (Exception exc)
            {
                // assert 1
                Assert.Fail(exc.Message);
            }
            
            // assert 2
            _rmqcCoreMock.Verify(x => x.Disconnect(), Times.Once);
        }
        
        /// <summary>
        /// Testes the ReConnect method. (Test 1)
        /// </summary>
        [Fact]
        public void RmqcWrapperTest_ReConnectTest1()
        {
            // arrange
            Exception? e;
            _rmqcCoreMock.Setup(x => x.Disconnect());
            _rmqcCoreMock.Setup(x => x.Connect(out e)).Returns(true);

            // act
            var res = _sut.ReConnect(); 
            
            // assert
            res.Should().BeTrue();
            _rmqcCoreMock.Verify(x => x.Disconnect(), Times.Once);
            _rmqcCoreMock.Verify(x => x.Connect(out It.Ref<Exception?>.IsAny), Times.Once);
        }
        
        /// <summary>
        /// Testes the ReConnect method. (Test 2)
        /// </summary>
        [Fact]
        public void RmqcWrapperTest_ReConnectTest2()
        {
            // arrange
            Exception? e;
            _rmqcCoreMock.Setup(x => x.Disconnect());
            _rmqcCoreMock.Setup(x => x.Connect(out e)).Returns(true);

            // act
            var res = _sut.ReConnect(out var resExc); 
            
            // assert
            res.Should().BeTrue();
            resExc.Should().BeNull();
            _rmqcCoreMock.Verify(x => x.Disconnect(), Times.Once);
            _rmqcCoreMock.Verify(x => x.Connect(out It.Ref<Exception?>.IsAny), Times.Once);
        }
        
        /// <summary>
        /// Testes the WriteMsg method. (Test 1)
        /// </summary>
        [Fact]
        public void RmqcWrapperTest_WriteMsgTest1()
        {
            // arrange
            Exception? e;
            var m = new Message();
            _rmqcCoreMock.Setup(x => x.WriteMsg(m,out e)).Returns(true);

            // act
            var res = _sut.WriteMsg(m); 
            
            // assert
            res.Should().BeTrue();
            _rmqcCoreMock.Verify(x => 
                x.WriteMsg(It.IsAny<Message>(), out It.Ref<Exception?>.IsAny), Times.Once);
        }
        
        /// <summary>
        /// Testes the WriteMsg method. (Test 2)
        /// </summary>
        [Fact]
        public void RmqcWrapperTest_WriteMsgTest2()
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
        /// Testes the ReadMsg method. (Test 1)
        /// </summary>
        [Fact]
        public void RmqcWrapperTest_ReadMsgTest1()
        {
            // arrange
            Exception? e;
            var m = new Message();
            _rmqcCoreMock.Setup(x => x.ReadMsg(out m, false, out e)).Returns(true);

            // act
            var res = _sut.ReadMsg(out m, false); 
            
            // assert
            res.Should().BeTrue();
            _rmqcCoreMock.Verify(x => 
                x.ReadMsg(out It.Ref<Message?>.IsAny!, false, out It.Ref<Exception?>.IsAny), Times.Once);
        }
        
        /// <summary>
        /// Testes the ReadMsg method. (Test 2)
        /// </summary>
        [Fact]
        public void RmqcWrapperTest_ReadMsgTest2()
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
        /// Testes the AckMsg method. (Test 1)
        /// </summary>
        [Fact]
        public void RmqcWrapperTest_AckMsgTest1()
        {
            // arrange
            Exception? e;
            var m = new Message();
            _rmqcCoreMock.Setup(x => x.AckMsg(m, out e)).Returns(true);

            // act
            var res = _sut.AckMsg(m); 
            
            // assert
            res.Should().BeTrue();
            _rmqcCoreMock.Verify(x => 
                x.AckMsg(It.IsAny<Message>(), out It.Ref<Exception?>.IsAny), Times.Once);
        }
        
        /// <summary>
        /// Testes the AckMsg method. (Test 2)
        /// </summary>
        [Fact]
        public void RmqcWrapperTest_AckMsgTest2()
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
        /// Testes the NackMsg method. (Test 1)
        /// </summary>
        [Fact]
        public void RmqcWrapperTest_NackMsgTest1()
        {
            // arrange
            Exception? e;
            var m = new Message();
            _rmqcCoreMock.Setup(x => x.NackMsg(m, false, out e)).Returns(true);

            // act
            var res = _sut.NackMsg(m, false); 
            
            // assert
            res.Should().BeTrue();
            _rmqcCoreMock.Verify(x => 
                x.NackMsg(It.IsAny<Message>(), false, out It.Ref<Exception?>.IsAny), Times.Once);
        }
        
        /// <summary>
        /// Testes the NackMsg method. (Test 2)
        /// </summary>
        [Fact]
        public void RmqcWrapperTest_NackMsgTest2()
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
        /// Testes the WaitForWriteConfirm method. (Test 1)
        /// </summary>
        [Fact]
        public void RmqcWrapperTest_WaitForWriteConfirmTest1()
        {
            // arrange
            Exception? e;
            var t = new TimeSpan();
            _rmqcCoreMock.Setup(x => x.WaitForWriteConfirm(t, out e)).Returns(true);

            // act
            var res = _sut.WaitForWriteConfirm(t); 
            
            // assert
            res.Should().BeTrue();
            _rmqcCoreMock.Verify(x => 
                x.WaitForWriteConfirm(It.IsAny<TimeSpan>(), out It.Ref<Exception?>.IsAny), Times.Once);
        }
        
        /// <summary>
        /// Testes the WaitForWriteConfirm method. (Test 2)
        /// </summary>
        [Fact]
        public void RmqcWrapperTest_WaitForWriteConfirmTest2()
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
        /// Testes the QueuedMsgs method. (Test 1)
        /// </summary>
        [Fact]
        public void RmqcWrapperTest_QueuedMsgsTest1()
        {
            // arrange
            Exception? e;
            uint? a = 0;
            _rmqcCoreMock.Setup(x => x.QueuedMsgs(out a, out e)).Returns(true);

            // act
            var res = _sut.QueuedMsgs(out a); 
            
            // assert
            res.Should().BeTrue();
            _rmqcCoreMock.Verify(x => 
                x.QueuedMsgs(out It.Ref<uint?>.IsAny, out It.Ref<Exception?>.IsAny), Times.Once);
        }
        
        /// <summary>
        /// Testes the QueuedMsgs method. (Test 2)
        /// </summary>
        [Fact]
        public void RmqcWrapperTest_QueuedMsgsTest2()
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

        #endregion
        
        #endregion
    }
}


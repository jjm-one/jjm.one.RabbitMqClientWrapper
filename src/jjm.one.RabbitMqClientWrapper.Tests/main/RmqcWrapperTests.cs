
using System;
using FluentAssertions;
using jjm.one.RabbitMqClientWrapper.main;
using jjm.one.RabbitMqClientWrapper.main.core;
using jjm.one.RabbitMqClientWrapper.types;
using jjm.one.RabbitMqClientWrapper.types.di;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit.Sdk;

namespace jjm.one.RabbitMqClientWrapper.Tests.main
{
    public class RmqcWrapperTests
    {
        #region private members

        private readonly RmqcWrapper _sut;
        private readonly Mock<IRmqcCore> _rmqcCoreMock;
        private readonly Mock<ILogger<RmqcWrapper>> _rmqcWrapperLoggingMock;

        #endregion
        public RmqcWrapperTests()
        {
            _rmqcCoreMock = new Mock<IRmqcCore>();
            _rmqcWrapperLoggingMock = new Mock<ILogger<RmqcWrapper>>();
            _sut = new RmqcWrapper(_rmqcCoreMock.Object, _rmqcWrapperLoggingMock.Object, 
                new DiSimpleTypeWrappersEnableWrapperLogging());
        }

        #region tests

        [Fact]
        public void Init_Test()
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
        
        [Fact]
        public void DeInit_Test()
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
        
        [Fact]
        public void Connect_Test1()
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
        
        [Fact]
        public void Connect_Test2()
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
        
        [Fact]
        public void Disconnect_Test()
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
        
        [Fact]
        public void ReConnect_Test1()
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
        
        [Fact]
        public void ReConnect_Test2()
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
        
        [Fact]
        public void WriteMsg_Test1()
        {
            // arrange
            Exception? e;
            Message m = new Message();
            _rmqcCoreMock.Setup(x => x.WriteMsg(m,out e)).Returns(true);

            // act
            var res = _sut.WriteMsg(m); 
            
            // assert
            res.Should().BeTrue();
            _rmqcCoreMock.Verify(x => 
                x.WriteMsg(It.IsAny<Message>(), out It.Ref<Exception?>.IsAny), Times.Once);
        }
        
        [Fact]
        public void WriteMsg_Test2()
        {
            // arrange
            Exception? e;
            Message m = new Message();
            _rmqcCoreMock.Setup(x => x.WriteMsg(m,out e)).Returns(true);

            // act
            var res = _sut.WriteMsg(m, out var resExc); 
            
            // assert
            res.Should().BeTrue();
            resExc.Should().BeNull();
            _rmqcCoreMock.Verify(x => 
                x.WriteMsg(It.IsAny<Message>(), out It.Ref<Exception?>.IsAny), Times.Once);
        }
        
        [Fact]
        public void ReadMsg_Test1()
        {
            // arrange
            Exception? e;
            Message? m = new Message();
            _rmqcCoreMock.Setup(x => x.ReadMsg(out m, false, out e)).Returns(true);

            // act
            var res = _sut.ReadMsg(out m, false); 
            
            // assert
            res.Should().BeTrue();
            _rmqcCoreMock.Verify(x => 
                x.ReadMsg(out It.Ref<Message?>.IsAny!, false, out It.Ref<Exception?>.IsAny), Times.Once);
        }
        
        [Fact]
        public void ReadMsg_Test2()
        {
            // arrange
            Exception? e;
            Message? m = new Message();
            _rmqcCoreMock.Setup(x => x.ReadMsg(out m, false, out e)).Returns(true);

            // act
            var res = _sut.ReadMsg(out m, false, out var resExc); 
            
            // assert
            res.Should().BeTrue();
            resExc.Should().BeNull();
            _rmqcCoreMock.Verify(x => 
                x.ReadMsg(out It.Ref<Message?>.IsAny, false, out It.Ref<Exception?>.IsAny), Times.Once);
        }
        
        [Fact]
        public void AckMsg_Test1()
        {
            // arrange
            Exception? e;
            Message? m = new Message();
            _rmqcCoreMock.Setup(x => x.AckMsg(m, out e)).Returns(true);

            // act
            var res = _sut.AckMsg(m); 
            
            // assert
            res.Should().BeTrue();
            _rmqcCoreMock.Verify(x => 
                x.AckMsg(It.IsAny<Message>(), out It.Ref<Exception?>.IsAny), Times.Once);
        }
        
        [Fact]
        public void AckMsg_Test2()
        {
            // arrange
            Exception? e;
            Message? m = new Message();
            _rmqcCoreMock.Setup(x => x.AckMsg(m, out e)).Returns(true);

            // act
            var res = _sut.AckMsg(m, out var resExc); 
            
            // assert
            res.Should().BeTrue();
            resExc.Should().BeNull();
            _rmqcCoreMock.Verify(x => 
                x.AckMsg(It.IsAny<Message>(), out It.Ref<Exception?>.IsAny), Times.Once);
        }
        
        [Fact]
        public void NackMsg_Test1()
        {
            // arrange
            Exception? e;
            Message? m = new Message();
            _rmqcCoreMock.Setup(x => x.NackMsg(m, false, out e)).Returns(true);

            // act
            var res = _sut.NackMsg(m, false); 
            
            // assert
            res.Should().BeTrue();
            _rmqcCoreMock.Verify(x => 
                x.NackMsg(It.IsAny<Message>(), false, out It.Ref<Exception?>.IsAny), Times.Once);
        }
        
        [Fact]
        public void NackMsg_Test2()
        {
            // arrange
            Exception? e;
            Message? m = new Message();
            _rmqcCoreMock.Setup(x => x.NackMsg(m, false, out e)).Returns(true);

            // act
            var res = _sut.NackMsg(m, false, out var resExc); 
            
            // assert
            res.Should().BeTrue();
            resExc.Should().BeNull();
            _rmqcCoreMock.Verify(x => 
                x.NackMsg(It.IsAny<Message>(), false, out It.Ref<Exception?>.IsAny), Times.Once);
        }
        
        [Fact]
        public void WaitForWriteConfirm_Test1()
        {
            // arrange
            Exception? e;
            TimeSpan t = new TimeSpan();
            _rmqcCoreMock.Setup(x => x.WaitForWriteConfirm(t, out e)).Returns(true);

            // act
            var res = _sut.WaitForWriteConfirm(t); 
            
            // assert
            res.Should().BeTrue();
            _rmqcCoreMock.Verify(x => 
                x.WaitForWriteConfirm(It.IsAny<TimeSpan>(), out It.Ref<Exception?>.IsAny), Times.Once);
        }
        
        [Fact]
        public void WaitForWriteConfirm_Test2()
        {
            // arrange
            Exception? e;
            TimeSpan t = new TimeSpan();
            _rmqcCoreMock.Setup(x => x.WaitForWriteConfirm(t, out e)).Returns(true);

            // act
            var res = _sut.WaitForWriteConfirm(t, out var resExc); 
            
            // assert
            res.Should().BeTrue();
            resExc.Should().BeNull();
            _rmqcCoreMock.Verify(x => 
                x.WaitForWriteConfirm(It.IsAny<TimeSpan>(), out It.Ref<Exception?>.IsAny), Times.Once);
        }
        
        [Fact]
        public void QueuedMsgs_Test1()
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
        
        [Fact]
        public void QueuedMsgs_Test2()
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
    }
}


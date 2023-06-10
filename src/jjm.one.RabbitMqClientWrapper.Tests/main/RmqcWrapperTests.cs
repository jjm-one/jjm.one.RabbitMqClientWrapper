
using System;
using FluentAssertions;
using jjm.one.RabbitMqClientWrapper.main;
using jjm.one.RabbitMqClientWrapper.main.core;
using jjm.one.RabbitMqClientWrapper.types.di;
using Microsoft.Extensions.Logging;
using Moq;

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
            _rmqcCoreMock.Verify(x => x.Connect(out It.Ref<Exception>.IsAny!), Times.Once);
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
            _rmqcCoreMock.Verify(x => x.Connect(out It.Ref<Exception>.IsAny!), Times.Once);
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
            _rmqcCoreMock.Verify(x => x.Connect(out It.Ref<Exception>.IsAny!), Times.Once);
        }
        
        [Fact]
        public void ReConnect_Test2()
        {
            
        }
        
        [Fact]
        public void WriteMsg_Test1()
        {
            
        }
        
        [Fact]
        public void WriteMsg_Test2()
        {
            
        }
        
        [Fact]
        public void ReadMsg_Test1()
        {
            
        }
        
        [Fact]
        public void ReadMsg_Test2()
        {
            
        }
        
        [Fact]
        public void AckMsg_Test1()
        {
            
        }
        
        [Fact]
        public void AckMsg_Test2()
        {
            
        }
        
        [Fact]
        public void NackMsg_Test1()
        {
            
        }
        
        [Fact]
        public void NackMsg_Test2()
        {
            
        }
        
        [Fact]
        public void WaitForWriteConfirm_Test1()
        {
            
        }
        
        [Fact]
        public void WaitForWriteConfirm_Test2()
        {
            
        }
        
        [Fact]
        public void QueuedMsgs_Test1()
        {
            
        }
        
        [Fact]
        public void QueuedMsgs_Test2()
        {
            
        }

        #endregion
    }
}


using System;
using jjm.one.RabbitMqClientWrapper.main;
using jjm.one.RabbitMqClientWrapper.Tests.testtools;
using jjm.one.RabbitMqClientWrapper.types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace jjm.one.RabbitMqClientWrapper.Tests.main
{
    public class RmqcWrapperTests
    {

        #region private members

        private IHost diHost;

        #endregion

        #region ctor's

        public RmqcWrapperTests()
        {
            var builder = Host.CreateDefaultBuilder();

            builder.ConfigureServices(services =>
                services.AddRMQCTestCore(false, true, null, new Message(), 42));

            diHost = builder.Build();
        }

        #endregion

        #region tests

        [Fact]
        public void SettingsTest()
        {
            var rmqc1 = diHost.Services.GetService<RmqcWrapper>() ?? throw new NullReferenceException();

            var s1 = new Settings("A", 42, "B", "C", "D", "E", "F");

            rmqc1.Settings = s1;

            Assert.True(s1.Equals(rmqc1.Settings));
        }

        [Fact]
        public void ConnectedTest()
        {
            var rmqc1 = diHost.Services.GetService<RmqcWrapper>() ?? throw new NullReferenceException();
            var rmqc2 = diHost.Services.GetService<RmqcWrapper>() ?? throw new NullReferenceException();

            Assert.True(rmqc1.Connected);
            Assert.False(rmqc2.Connected);
        }

        [Fact]
        public void InitTest()
        {
            var rmqc1 = diHost.Services.GetService<RmqcWrapper>() ?? throw new NullReferenceException();

            rmqc1.Init();
        }

        [Fact]
        public void DeInitTest()
        {
            var rmqc1 = diHost.Services.GetService<RmqcWrapper>() ?? throw new NullReferenceException();

            rmqc1.Init();
        }

        [Fact]
        public void ConnectTest1()
        {
            var rmqc1 = diHost.Services.GetService<RmqcWrapper>() ?? throw new NullReferenceException();
            var rmqc2 = diHost.Services.GetService<RmqcWrapper>() ?? throw new NullReferenceException();

            Assert.True(rmqc1.Connect());
            Assert.False(rmqc2.Connect());
        }

        [Fact]
        public void ConnectTest2()
        {

        }

        [Fact]
        public void DisconnectTest()
        {

        }

        [Fact]
        public void ReConnectTest1()
        {

        }

        [Fact]
        public void ReConnectTest2()
        {

        }

        [Fact]
        public void WriteMsgTest1()
        {

        }

        [Fact]
        public void WriteMsgTest2()
        {

        }

        [Fact]
        public void ReadMsgTest1()
        {

        }

        [Fact]
        public void ReadMsgTest2()
        {

        }

        [Fact]
        public void AckMsgTest1()
        {

        }

        [Fact]
        public void AckMsgTest2()
        {

        }

        [Fact]
        public void NackMsgTest1()
        {

        }

        [Fact]
        public void NackMsgTest2()
        {

        }

        [Fact]
        public void WaitForWriteConfirmTest1()
        {

        }

        [Fact]
        public void WaitForWriteConfirmTest2()
        {

        }

        [Fact]
        public void QueuedMsgsTest1()
        {

        }

        [Fact]
        public void QueuedMsgsTest2()
        {

        }

        #endregion
    }
}


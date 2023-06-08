using System;
using jjm.one.RabbitMqClientWrapper.main.core;
using jjm.one.RabbitMqClientWrapper.types;
using Microsoft.Extensions.DependencyInjection;

namespace jjm.one.RabbitMqClientWrapper.Tests.testtools
{
    public class RmqcTestCore : IRmqcCore
	{
        #region private members

        private Settings settings = new Settings();

        #endregion

        #region public members

        public RmqcTestCoreArgs Args = new RmqcTestCoreArgs();

        public Settings Settings { get; set; } = new Settings();

        public bool Connected => Args.ResultOnReturn;

        #endregion

        #region ctor's

        public RmqcTestCore(RmqcTestCoreArgs args)
        {
            Args = args;
        }

        #endregion

        #region public methods

        public bool AckMsg(Message message, out Exception? exception)
        {
            exception = Args.ExceptionOnReturn;

            return Args.ResultOnReturn;
        }

        public bool Connect(out Exception? exception)
        {
            exception = Args.ExceptionOnReturn;

            return Args.ResultOnReturn;
        }

        public void DeInit()
        {
            return;
        }

        public void Disconnect()
        {
            return;
        }

        public void Init()
        {
            return;
        }

        public bool NackMsg(Message message, bool requeue, out Exception? exception)
        {
            exception = Args.ExceptionOnReturn;

            return Args.ResultOnReturn;
        }

        public bool QueuedMsgs(out uint? amount, out Exception? exception)
        {
            exception = Args.ExceptionOnReturn;
            amount = Args.AmountOnReturn;

            return Args.ResultOnReturn;
        }

        public bool ReadMsg(out Message? message, bool autoAck, out Exception? exception)
        {
            exception = Args.ExceptionOnReturn;
            message = Args.MessageOnReturn;

            return Args.ResultOnReturn;
        }

        public bool WaitForWriteConfirm(TimeSpan timeout, out Exception? exception)
        {
            exception = Args.ExceptionOnReturn;

            return Args.ResultOnReturn;
        }

        public bool WriteMsg(Message message, out Exception? exception)
        {
            exception = Args.ExceptionOnReturn;

            return Args.ResultOnReturn;
        }

        #endregion
    }
}

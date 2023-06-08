﻿using System;
using jjm.one.RabbitMqClientWrapper.main.core;
using jjm.one.RabbitMqClientWrapper.types;

namespace jjm.one.RabbitMqClientWrapper.main
{
    public interface IRmqcWrapper
    {
        #region public memberss

        public Settings Settings { get; set; }
        public bool Connected { get; }

        #endregion

        #region public methods

        public void Init();
        public void DeInit();

        public bool Connect();
        public bool Connect(out Exception? exception);
        public void Disconnect();
        public bool ReConnect();
        public bool ReConnect(out Exception? exception);

        public bool WriteMsg(Message message);
        public bool WriteMsg(Message message, out Exception? exception);
        public bool ReadMsg(out Message? message, bool autoAck);
        public bool ReadMsg(out Message? message, bool autoAck, out Exception? exception);

        public bool AckMsg(Message message);
        public bool AckMsg(Message message, out Exception? exception);
        public bool NackMsg(Message message, bool requeue);
        public bool NackMsg(Message message, bool requeue, out Exception? exception);

        public bool WaitForWriteConfirm(TimeSpan timeout);
        public bool WaitForWriteConfirm(TimeSpan timeout, out Exception? exception);

        public bool QueuedMsgs(out uint? amount);
        public bool QueuedMsgs(out uint? amount, out Exception? exception);

        #endregion
    }
}
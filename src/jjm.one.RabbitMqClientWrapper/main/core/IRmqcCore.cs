using System;
using jjm.one.RabbitMqClientWrapper.types;

namespace jjm.one.RabbitMqClientWrapper.main.core
{
    public interface IRmqcCore
    {
        #region public members

        public Settings Settings { get; set; }
        public bool Connected { get; }

        #endregion

        #region public methods

        public void Init();
        public void DeInit();

        public bool Connect(out Exception? exception);
        public void Disconnect();

        public bool WriteMsg(Message message, out Exception? exception);
        public bool ReadMsg(out Message? message, bool autoAck, out Exception? exception);

        public bool AckMsg(Message message, out Exception? exception);
        public bool NackMsg(Message message, bool requeue, out Exception? exception);

        public bool WaitForWriteConfirm(TimeSpan timeout, out Exception? exception);

        public bool QueuedMsgs(out uint? amount, out Exception? exception);

        #endregion
    }
}
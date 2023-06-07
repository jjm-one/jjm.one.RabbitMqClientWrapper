using jjm.one.RabbitMqClientWrapper.types;

namespace jjm.one.RabbitMqClientWrapper.main.core
{
    internal interface IRabbitMqClientCore
    {
        #region public members

        public Settings Settings { get; set; }
        public ClientStatus ClientStatus { get; set; }

        #endregion

        #region public methods

        public bool Init();
        public bool Connect();
        public bool Disconnect();

        public bool WriteMsg(Message message);
        public bool WriteMsg(Message message, out ResultStatus resultStatus);
        public bool ReadMsg(out Message message);
        public bool ReadMsg(out Message message, out ResultStatus resultStatus);
        public bool AckMsg(Message message);
        public bool AckMsg(Message message, out ResultStatus resultStatus);

        #endregion



    }
}
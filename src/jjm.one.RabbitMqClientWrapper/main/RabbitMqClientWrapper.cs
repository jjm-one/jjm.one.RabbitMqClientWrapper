using jjm.one.RabbitMqClientWrapper.main.core;
using jjm.one.RabbitMqClientWrapper.types;
using Microsoft.Extensions.Logging;

namespace jjm.one.RabbitMqClientWrapper.main
{
    public class RabbitMqClientWrapper : IRabbitMqClientWrapper
    {
        #region private members

        private readonly IRabbitMqClientCore _core;
        private readonly ILogger<RabbitMqClientWrapper> _logger;

        #endregion

        #region public members

        public Settings Settings
        {
            get => _core.Settings;
            private set => _core.Settings = value;
        }

        public ClientStatus ClientStatus => _core.ClientStatus;

        #endregion

        #region ctor's

        public RabbitMqClientWrapper(Settings settings, ILogger<RabbitMqClientWrapper>? logger = null)
        {
            _core = new RabbitMqClientCore(settings: settings, clientStatus: new ClientStatus());
            _logger = logger ?? new Logger<RabbitMqClientWrapper>(new LoggerFactory());
        }

        internal RabbitMqClientWrapper(IRabbitMqClientCore core, ILogger<RabbitMqClientWrapper> logger)
        {
            _core = core;
            _logger = logger;
        }

        #endregion

        #region public methods

        public bool Init()
        {
            return _core.Init();
        }

        public bool Connect()
        {
            return _core.Connect();
        }

        public bool Disconnect()
        {
            return _core.Disconnect();
        }

        public bool Reconnect()
        {
            var res = true;
            res &= _core.Connect();
            res &= _core.Disconnect();

            return res;
        }

        public bool WriteMsg(Message message)
        {
            return _core.WriteMsg(message);
        }

        public bool WriteMsg(Message message, out ResultStatus resultStatus)
        {
            return _core.WriteMsg(message, out resultStatus);
        }

        public bool ReadMsg(out Message message)
        {
            return _core.ReadMsg(out message);
        }

        public bool ReadMsg(out Message message, out ResultStatus resultStatus)
        {
            return _core.ReadMsg(out message, out resultStatus);
        }

        public bool AckMsg(Message message)
        {
            return _core.AckMsg(message);
        }

        public bool AckMsg(Message message, out ResultStatus resultStatus)
        {
            return _core.AckMsg(message, out resultStatus);
        }

        #endregion


    }
}

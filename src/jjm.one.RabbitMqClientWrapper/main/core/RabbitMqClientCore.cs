using System.Reflection;
using jjm.one.Microsoft.Extensions.Logging.Helpers;
using jjm.one.RabbitMqClientWrapper.types;
using Microsoft.Extensions.Logging;

namespace jjm.one.RabbitMqClientWrapper.main.core
{
    internal class RabbitMqClientCore : IRabbitMqClientCore
    {
        #region private members

        private readonly ILogger<RabbitMqClientCore> _logger;

        #endregion


        #region public members

        public Settings Settings { get; set; }
        public ClientStatus ClientStatus { get; set; }

        #endregion

        #region ctor's

        public RabbitMqClientCore(Settings settings, ClientStatus clientStatus, ILogger<RabbitMqClientCore>? logger = null)
        {
            // init global vars
            Settings = settings;
            ClientStatus = clientStatus;
            _logger = logger ?? new Logger<RabbitMqClientCore>(new LoggerFactory());

            // log fct call
            _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);
        }

        #endregion

        public bool Init()
        {
            // log fct call
            _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            throw new NotImplementedException();
        }

        public bool Connect()
        {
            // log fct call
            _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);
            
            throw new NotImplementedException();
        }

        public bool Disconnect()
        {
            // log fct call
            _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            throw new NotImplementedException();
        }

        public bool WriteMsg(Message message)
        {
            // log fct call
            _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            throw new NotImplementedException();
        }

        public bool WriteMsg(Message message, out ResultStatus resultStatus)
        {
            // log fct call
            _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            throw new NotImplementedException();
        }

        public bool ReadMsg(out Message message)
        {
            // log fct call
            _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            throw new NotImplementedException();
        }

        public bool ReadMsg(out Message message, out ResultStatus resultStatus)
        {
            // log fct call
            _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);
            
            throw new NotImplementedException();
        }

        public bool AckMsg(Message message)
        {
            // log fct call
            _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            throw new NotImplementedException();
        }

        public bool AckMsg(Message message, out ResultStatus resultStatus)
        {
            // log fct call
            _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            throw new NotImplementedException();
        }
    }
}

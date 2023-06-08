using jjm.one.Microsoft.Extensions.Logging.Helpers;
using jjm.one.RabbitMqClientWrapper.main.core;
using jjm.one.RabbitMqClientWrapper.types;
using jjm.one.RabbitMqClientWrapper.types.di;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace jjm.one.RabbitMqClientWrapper.main
{
    public class RmqcWrapper : IRmqcWrapper
    {
        #region private members

        private readonly IRmqcCore _core;
        private readonly ILogger<RmqcWrapper> _logger;
        private readonly bool _enableLogging;

        #endregion

        #region public members

        public Settings Settings
        {
            get
            {
                // log fct call
                if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

                return _core.Settings;
            }
            set
            {
                // log fct call
                if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

                _core.Settings = value;
            }
        }

        public bool Connected
        {
            get
            {
                // log fct call
                if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

                return _core.Connected;
            }
        }

        #endregion

        #region ctor's

        public RmqcWrapper(IRmqcCore core, ILogger<RmqcWrapper> logger, DiSimpleTypeWrappersEnableWrapperLogging? enableLoggin = null)
        {
            _core = core;
            _logger = logger;
            _enableLogging = enableLoggin?.EnableLogging ?? false;

            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);
        }

        public RmqcWrapper(Settings settings, bool enableWrapperLogging = false, bool enableCoreLogging = false)
        {
            _core = new RmqcCore(settings, new Logger<RmqcCore>(new LoggerFactory()), new DiSimpleTypeWrappersEnableCoreLogging(enableCoreLogging));
            _logger = new Logger<RmqcWrapper>(new LoggerFactory());
            _enableLogging = enableWrapperLogging;

            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);
        }

        #endregion

        #region public methods

        public void Init()
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            _core.Init();
        }

        public void DeInit()
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            _core.DeInit();
        }

        public bool Connect()
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            return Connect(out _);
        }

        public bool Connect(out Exception? exception)
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            return _core.Connect(out exception);
        }

        public void Disconnect()
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            _core.Disconnect();
        }

        public bool ReConnect()
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            return ReConnect(out _);
        }

        public bool ReConnect(out Exception? exception)
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            _core.Disconnect();
            return _core.Connect(out exception);
        }

        public bool WriteMsg(Message message)
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            return WriteMsg(message, out _);
        }

        public bool WriteMsg(Message message, out Exception? exception)
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            return _core.WriteMsg(message, out exception);
        }

        public bool ReadMsg(out Message? message, bool autoAck)
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            return ReadMsg(out message, autoAck, out _);
        }

        public bool ReadMsg(out Message? message, bool autoAck, out Exception? exception)
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            return _core.ReadMsg(out message, autoAck, out exception);
        }

        public bool AckMsg(Message message)
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            return AckMsg(message, out _);
        }

        public bool AckMsg(Message message, out Exception? exception)
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            return _core.AckMsg(message, out exception);
        }

        public bool NackMsg(Message message, bool requeue)
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            return NackMsg(message, requeue, out _);
        }

        public bool NackMsg(Message message, bool requeue, out Exception? exception)
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            return _core.NackMsg(message, requeue, out exception);
        }

        public bool WaitForWriteConfirm(TimeSpan timeout)
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            return WaitForWriteConfirm(timeout, out _);
        }

        public bool WaitForWriteConfirm(TimeSpan timeout, out Exception? exception)
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            return _core.WaitForWriteConfirm(timeout, out exception);
        }

        public bool QueuedMsgs(out uint? amount)
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            return QueuedMsgs(out amount, out _);
        }

        public bool QueuedMsgs(out uint? amount, out Exception? exception)
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            return _core.QueuedMsgs(out amount, out exception);
        }

        #endregion
    }
}

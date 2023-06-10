using System;
using System.Reflection;
using jjm.one.Microsoft.Extensions.Logging.Helpers;
using jjm.one.RabbitMqClientWrapper.types;
using jjm.one.RabbitMqClientWrapper.types.di;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace jjm.one.RabbitMqClientWrapper.main.core
{
    /// <summary>
    /// This class implements the <see cref="IRmqcCore"/> interface for a RabbitMQ server.
    /// </summary>
    internal class RmqcCore : IRmqcCore
    {
        #region private members

        private Settings _settings;
        private readonly ILogger<RmqcCore> _logger;
        private readonly bool _enableLogging;

        private IConnectionFactory? _connectionFactory;
        private IConnection? _connection;
        private IModel? _channel;

        #endregion

        #region public members

        /// <summary>
        /// This object contains the settings for the RabbitMQ client.
        /// Note:
        /// Changing the <see cref="Settings"/> object of a connected client will result in the disconnection from the server.
        /// </summary>
        public Settings Settings
        {
            get
            {
                // log fct call
                if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

                return _settings;
            }
            set
            {
                // log fct call
                if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

                // check im nothing changed
                if (_settings.Equals(value))
                {
                    return;
                }
                
                // disconnect & re-init the connection
                Disconnect();
                _settings = value;
                Init();
            }
        }

        /// <inheritdoc />
        public bool Connected
        {
            get
            {
                // check connection factory
                if (_connectionFactory is null)
                {
                    return false;
                }

                // check connection
                if (_connection is null || !_connection.IsOpen)
                {
                    return false;
                }

                // check channel
                if (_channel is null || !_channel.IsOpen)
                {
                    return false;
                }

                // connected!
                return true;
            }
        }

        #endregion

        #region ctor's

        /// <summary>
        /// This is a parameterised constructor for the <see cref="RmqcCore"/> class.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="logger"></param>
        /// <param name="enableLogging"></param>
        public RmqcCore(Settings settings, ILogger<RmqcCore> logger, DiSimpleTypeWrappersEnableCoreLogging? enableLogging = null)
        {
            // init global vars
            _settings = settings;
            _logger = logger;
            _enableLogging = enableLogging?.EnableLogging ?? false;

            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);
        }

        /// <summary>
        /// This is a parameterised constructor for the <see cref="RmqcCore"/> class.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <param name="enableLogging"></param>
        public RmqcCore(IOptions<Settings> options, ILogger<RmqcCore> logger, DiSimpleTypeWrappersEnableCoreLogging? enableLogging = null)
        {
            // init global vars
            _settings = options.Value;
            _logger = logger;
            _enableLogging = enableLogging?.EnableLogging ?? false;

            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);
        }

        #endregion

        /// <inheritdoc />
        public void Init()
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            // create the new connection factory
            _connectionFactory = new ConnectionFactory
            {
                HostName = Settings.Hostname,
                Port = Settings.Port,
                UserName = Settings.Username,
                Password = Settings.Password,
                VirtualHost = Settings.VHost
            };
        }

        /// <inheritdoc />
        public void DeInit()
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            // create the new connection factory
            _connectionFactory = null;
        }

        /// <inheritdoc />
        public bool Connect(out Exception? exception)
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            // init output
            var res = true;
            exception = null;

            try
            {
                // check connection factory
                if (_connectionFactory is null)
                {
                    throw new NullReferenceException(
                        $"The {nameof(IConnectionFactory)} is null! " +
                        $"Maybe the {nameof(RmqcCore)} was not initialized properly.");
                }

                // create the connection
                _connection = _connectionFactory.CreateConnection();

                // check connection
                if (_connection is null)
                {
                    throw new NullReferenceException(
                        $"The {nameof(IConnection)} is null! " +
                        $"Maybe the {nameof(RmqcCore)} was not initialized properly.");
                }

                // create the channel
                _channel = _connection.CreateModel();

                // check channel
                if (_channel is null)
                {
                    throw new NullReferenceException(
                        $"The {nameof(IModel)} ({nameof(_channel)}) is null! " +
                        $"Maybe the {nameof(RmqcCore)} was not initialized properly.");
                }
            }
            catch(Exception exc)
            {
                // log exception
                if (_enableLogging) _logger.LogExcInFctCall(exc, GetType(), MethodBase.GetCurrentMethod(), exc.Message, LogLevel.Warning);

                // disconnect
                Disconnect();

                // set output 
                exception = exc;
                res = false;
            }

            return res;
        }

        /// <inheritdoc />
        public void Disconnect()
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            try
            {
                // disconnect channel
                if (_channel is not null)
                {
                    if (_channel.IsOpen)
                    {
                        _channel.Close();
                    }

                    _channel.Dispose();
                    _channel = null;
                }

                // disconnect connection
                if (_connection is not null)
                {
                    if (_connection.IsOpen)
                    {
                        _connection.Close();
                    }

                    _connection.Dispose();
                    _connection = null;
                }
            }
            catch (Exception exc)
            {
                // log exception
                if (_enableLogging) _logger.LogExcInFctCall(exc, GetType(), MethodBase.GetCurrentMethod(), exc.Message, LogLevel.Warning);

                // re-throw exception
                throw;
            }
        }

        /// <inheritdoc />
        public bool WriteMsg(Message message, out Exception? exception)
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            // init output
            var res = true;
            exception = null;

            try
            {
                // check connection to server
                if (!Connected)
                {
                    throw new InvalidOperationException("Client must be initialized and connected to perform this operation!");
                }

                // write message
                _channel?.BasicPublish(Settings.Exchange, message.RoutingKey, message.BasicProperties ?? null, message.Body ?? null);
            }
            catch (Exception exc)
            {
                // log exception
                if (_enableLogging) _logger.LogExcInFctCall(exc, GetType(), MethodBase.GetCurrentMethod(), exc.Message, LogLevel.Warning);

                // set output 
                exception = exc;
                res = false;
            }

            return res;
        }

        /// <inheritdoc />
        public bool ReadMsg(out Message? message, bool autoAck, out Exception? exception)
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            // init output
            var res = true;
            exception = null;
            message = null;

            try
            {
                // check connection to server
                if (!Connected)
                {
                    throw new InvalidOperationException("Client must be initialized and connected to perform this operation!");
                }

                // read message
                var msg = _channel?.BasicGet(Settings.Queue, autoAck);

                // check message
                if (msg is null)
                {
                    res = false;
                }
                else
                {
                    message = new Message(msg);
                }
            }
            catch (Exception exc)
            {
                // log exception
                if (_enableLogging) _logger.LogExcInFctCall(exc, GetType(), MethodBase.GetCurrentMethod(), exc.Message, LogLevel.Warning);

                // set output 
                exception = exc;
                res = false;
            }

            return res;
        }

        /// <inheritdoc />
        public bool AckMsg(Message message, out Exception? exception)
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            // init output
            var res = true;
            exception = null;

            try
            {
                // check connection to server
                if (!Connected)
                {
                    throw new InvalidOperationException("Client must be initialized and connected to perform this operation!");
                }

                // send ack
                _channel?.BasicAck(message.DeliveryTag, false);
            }
            catch (Exception exc)
            {
                // log exception
                if (_enableLogging) _logger.LogExcInFctCall(exc, GetType(), MethodBase.GetCurrentMethod(), exc.Message, LogLevel.Warning);

                // set output 
                exception = exc;
                res = false;
            }

            return res;
        }

        /// <inheritdoc />
        public bool NackMsg(Message message, bool requeue, out Exception? exception)
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            // init output
            var res = true;
            exception = null;

            try
            {
                // check connection to server
                if (!Connected)
                {
                    throw new InvalidOperationException("Client must be initialized and connected to perform this operation!");
                }

                // send unack
                _channel?.BasicNack(message.DeliveryTag, false, requeue);
            }
            catch (Exception exc)
            {
                // log exception
                if (_enableLogging) _logger.LogExcInFctCall(exc, GetType(), MethodBase.GetCurrentMethod(), exc.Message, LogLevel.Warning);

                // set output 
                exception = exc;
                res = false;
            }

            return res;
        }

        /// <inheritdoc />
        public bool WaitForWriteConfirm(TimeSpan timeout, out Exception? exception)
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            // init output
            var res = true;
            exception = null;

            try
            {
                // check connection to server
                if (!Connected)
                {
                    throw new InvalidOperationException("Client must be initialized and connected to perform this operation!");
                }

                // send unack
                res &= _channel?.WaitForConfirms(timeout) ?? false;
            }
            catch (Exception exc)
            {
                // log exception
                if (_enableLogging) _logger.LogExcInFctCall(exc, GetType(), MethodBase.GetCurrentMethod(), exc.Message, LogLevel.Warning);

                // set output 
                exception = exc;
                res = false;
            }

            return res;
        }

        /// <inheritdoc />
        public bool QueuedMsgs(out uint? amount, out Exception? exception)
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            // init output
            var res = true;
            exception = null;
            amount = null;

            try
            {
                // check connection to server
                if (!Connected)
                {
                    throw new InvalidOperationException("Client must be initialized and connected to perform this operation!");
                }

                // get number of messages in queue
                amount = _channel?.MessageCount(Settings.Queue);
            }
            catch (Exception exc)
            {
                // log exception
                if (_enableLogging) _logger.LogExcInFctCall(exc, GetType(), MethodBase.GetCurrentMethod(), exc.Message, LogLevel.Warning);

                // set output 
                exception = exc;
                res = false;
            }

            return res;
        }
    }
}

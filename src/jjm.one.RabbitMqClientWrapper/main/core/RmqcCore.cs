using System;
using System.Reflection;
using jjm.one.Microsoft.Extensions.Logging.Helpers;
using jjm.one.RabbitMqClientWrapper.di.core;
using jjm.one.RabbitMqClientWrapper.types;
using jjm.one.RabbitMqClientWrapper.types.di;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace jjm.one.RabbitMqClientWrapper.main.core
{
    public class RmqcCore : IRmqcCore
    {
        #region private members

        private Settings settings;
        private readonly ILogger<RmqcCore> _logger;
        private readonly bool _enableLogging;

        private IConnectionFactory? connectionFactory;
        private IConnection? connection;
        private IModel? channel;

        #endregion

        #region public members

        public Settings Settings
        {
            get
            {
                // log fct call
                if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

                return settings;
            }
            set
            {
                // log fct call
                if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

                if (!settings.Equals(value))
                {
                    this.Disconnect();
                    settings = value;
                    this.Init();
                }
            }
        }

        public bool Connected
        {
            get
            {
                // check connection factory
                if (connectionFactory is null)
                {
                    return false;
                }

                // check connecction
                if (connection is null || !connection.IsOpen)
                {
                    return false;
                }

                // check channel
                if (channel is null || !channel.IsOpen)
                {
                    return false;
                }

                // connected!
                return true;
            }
        }

        #endregion

        #region ctor's

        public RmqcCore(Settings settings, ILogger<RmqcCore> logger, DiSimpleTypeWrappersEnableCoreLogging? enableLogging = null)
        {
            // init global vars
            this.settings = settings;
            _logger = logger;
            _enableLogging = enableLogging?.EnableLogging ?? false;

            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);
        }

        public RmqcCore(IOptions<Settings> options, ILogger<RmqcCore> logger, DiSimpleTypeWrappersEnableCoreLogging? enableLogging = null)
        {
            // init global vars
            settings = options.Value;
            _logger = logger;
            _enableLogging = enableLogging?.EnableLogging ?? false;

            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);
        }

        #endregion

        public void Init()
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            // create the new connection factory
            connectionFactory = new ConnectionFactory
            {
                HostName = Settings.Hostname,
                Port = Settings.Port,
                UserName = Settings.Username,
                Password = Settings.Password,
                VirtualHost = Settings.VHost
            };
        }

        public void DeInit()
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            // create the new connection factory
            connectionFactory = null;
        }

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
                if (connectionFactory is null)
                {
                    throw new NullReferenceException(
                        $"The {nameof(IConnectionFactory)} is null! " +
                        $"Maybe the {nameof(RmqcCore)} was not initialized properly.");
                }

                // create the connection
                connection = connectionFactory.CreateConnection();

                // check connection
                if (connection is null)
                {
                    throw new NullReferenceException(
                        $"The {nameof(IConnection)} is null! " +
                        $"Maybe the {nameof(RmqcCore)} was not initialized properly.");
                }

                // chreate the channel
                channel = connection.CreateModel();

                // check channel
                if (channel is null)
                {
                    throw new NullReferenceException(
                        $"The {nameof(IModel)} ({nameof(channel)}) is null! " +
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

        public void Disconnect()
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            try
            {
                // disconnect channel
                if (channel is not null)
                {
                    if (channel.IsOpen)
                    {
                        channel.Close();
                    }

                    channel.Dispose();
                    channel = null;
                }

                // disconnect connection
                if (connection is not null)
                {
                    if (connection.IsOpen)
                    {
                        connection.Close();
                    }

                    connection.Dispose();
                    connection = null;
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
                channel?.BasicPublish(Settings.Exchange, message.RoutingKey, message.BasicProperties ?? null, message.Body ?? null);
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
                var msg = channel?.BasicGet(Settings.Queue, autoAck);

                // check message
                if (msg is null)
                {
                    res &= false;
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
                channel?.BasicAck(message.DeliveryTag, false);
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
                channel?.BasicNack(message.DeliveryTag, false, requeue);
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
                res &= channel?.WaitForConfirms(timeout) ?? false;
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
                amount = channel?.MessageCount(Settings.Queue);
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

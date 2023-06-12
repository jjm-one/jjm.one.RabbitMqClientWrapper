using System;
using System.Reflection;
using jjm.one.Microsoft.Extensions.Logging.Helpers;
using jjm.one.RabbitMqClientWrapper.types;
using jjm.one.RabbitMqClientWrapper.types.exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace jjm.one.RabbitMqClientWrapper.main.core;

/// <summary>
/// This class implements the <see cref="IRmqcCore"/> interface for a RabbitMQ server.
/// </summary>
internal class RmqcCore : IRmqcCore
{
    #region private members

    private Settings _settings;
    private readonly ILogger<RmqcCore>? _logger;

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
            _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            return _settings;
        }
        set
        {
            // log fct call
            _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

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
    /// A parameterised constructor of the <see cref="RmqcCore"/> class.
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="logger"></param>
    [ActivatorUtilitiesConstructor]
    public RmqcCore(Settings settings, ILogger<RmqcCore>? logger)
    {
        // init global vars
        _settings = settings;
        _logger = logger;

        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);
    }

    /// <summary>
    /// A parameterised constructor of the <see cref="RmqcCore"/> class.
    /// For unit-tests only!
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="logger"></param>
    /// <param name="connectionFactory"></param>
    /// <param name="connection"></param>
    /// <param name="channel"></param>
    internal RmqcCore(Settings settings, ILogger<RmqcCore>? logger,
        IConnectionFactory? connectionFactory, IConnection? connection, IModel? channel)
    {
        // init global vars
        _settings = settings;
        _logger = logger;
        _connectionFactory = connectionFactory;
        _connection = connection;
        _channel = channel;

        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);
    }

    #endregion

    /// <inheritdoc />
    public void Init()
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

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
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // create the new connection factory
        _connectionFactory = null;
    }

    /// <inheritdoc />
    public bool Connect(out Exception? exception)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // init output
        var res = true;
        exception = null;

        try
        {
            // check connection factory
            if (_connectionFactory is null)
            {
                throw new NoConnectionFactoryException();
            }

            // create the connection
            _connection = _connectionFactory.CreateConnection();

            // check connection
            if (_connection is null)
            {
                throw new NoConnectionException();
            }

            // create the channel
            _channel = _connection.CreateModel();

            // check channel
            if (_channel is null)
            {
                throw new NoChannelException();
            }
        }
        catch(Exception exc)
        {
            // log exception
            _logger?.LogExcInFctCall(exc, GetType(), MethodBase.GetCurrentMethod(), exc.Message, LogLevel.Warning);

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
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

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
            _logger?.LogExcInFctCall(exc, GetType(), MethodBase.GetCurrentMethod(), exc.Message, LogLevel.Warning);

            // re-throw exception
            throw;
        }
    }

    /// <inheritdoc />
    public bool WriteMsg(Message message, out Exception? exception)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // init output
        var res = true;
        exception = null;

        try
        {
            // check connection to server
            if (!Connected)
            {
                throw new InvalidOperationException(nameof(WriteMsg));
            }

            // write message
            _channel?.BasicPublish(Settings.Exchange, message.RoutingKey, message.BasicProperties ?? null, message.Body ?? null);
        }
        catch (Exception exc)
        {
            // log exception
            _logger?.LogExcInFctCall(exc, GetType(), MethodBase.GetCurrentMethod(), exc.Message, LogLevel.Warning);

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
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // init output
        var res = true;
        exception = null;
        message = null;

        try
        {
            // check connection to server
            if (!Connected)
            {
                throw new InvalidOperationException(nameof(ReadMsg));
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
            _logger?.LogExcInFctCall(exc, GetType(), MethodBase.GetCurrentMethod(), exc.Message, LogLevel.Warning);

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
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // init output
        var res = true;
        exception = null;

        try
        {
            // check connection to server
            if (!Connected)
            {
                throw new InvalidOperationException(nameof(AckMsg));
            }

            // send ack
            _channel?.BasicAck(message.DeliveryTag, false);
        }
        catch (Exception exc)
        {
            // log exception
            _logger?.LogExcInFctCall(exc, GetType(), MethodBase.GetCurrentMethod(), exc.Message, LogLevel.Warning);

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
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // init output
        var res = true;
        exception = null;

        try
        {
            // check connection to server
            if (!Connected)
            {
                throw new InvalidOperationException(nameof(NackMsg));
            }

            // send nack
            _channel?.BasicNack(message.DeliveryTag, false, requeue);
        }
        catch (Exception exc)
        {
            // log exception
            _logger?.LogExcInFctCall(exc, GetType(), MethodBase.GetCurrentMethod(), exc.Message, LogLevel.Warning);

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
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // init output
        var res = true;
        exception = null;

        try
        {
            // check connection to server
            if (!Connected)
            {
                throw new InvalidOperationException(nameof(WaitForWriteConfirm));
            }

            // send nack
            res &= _channel?.WaitForConfirms(timeout) ?? false;
        }
        catch (Exception exc)
        {
            // log exception
            _logger?.LogExcInFctCall(exc, GetType(), MethodBase.GetCurrentMethod(), exc.Message, LogLevel.Warning);

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
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // init output
        var res = true;
        exception = null;
        amount = null;

        try
        {
            // check connection to server
            if (!Connected)
            {
                throw new InvalidOperationException(nameof(QueuedMsgs));
            }

            // get number of messages in queue
            amount = _channel?.MessageCount(Settings.Queue);
        }
        catch (Exception exc)
        {
            // log exception
            _logger?.LogExcInFctCall(exc, GetType(), MethodBase.GetCurrentMethod(), exc.Message, LogLevel.Warning);

            // set output 
            exception = exc;
            res = false;
        }

        return res;
    }
}

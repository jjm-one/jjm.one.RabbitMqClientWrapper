using System;
using System.Diagnostics;
using System.Reflection;
using jjm.one.Microsoft.Extensions.Logging.Helpers;
using jjm.one.RabbitMqClientWrapper.types;
using jjm.one.RabbitMqClientWrapper.types.events;
using jjm.one.RabbitMqClientWrapper.types.exceptions;
using jjm.one.RabbitMqClientWrapper.util;
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

    private RmqcSettings _settings;
    private readonly ILogger<RmqcCore>? _logger;

    private IConnectionFactory? _connectionFactory;
    private IConnection? _connection;
    private IModel? _channel;
    private bool _connected;

    #endregion

    #region public members

    /// <summary>
    /// This object contains the settings for the RabbitMQ client.
    /// Note:
    /// Changing the <see cref="Settings"/> object of a connected client will result in the disconnection from the server.
    /// </summary>
    public RmqcSettings Settings
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
            Disconnect(out _);
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
                // update the internal value in necessary
                if (_connected)
                {
                    Connected = false;
                }

                return false;
            }

            // check connection
            if (_connection is null || !_connection.IsOpen)
            {
                // update the internal value in necessary
                if (_connected)
                {
                    Connected = false;
                }

                return false;
            }

            // check channel
            if (_channel is null || !_channel.IsOpen)
            {
                // update the internal value in necessary
                if (_connected)
                {
                    Connected = false;
                }

                return false;
            }

            // update the internal value in necessary
            if (!_connected)
            {
                Connected = true;
            }

            // connected!
            return true;
        }

        private set
        {
            // check if the new value is equal to the old value
            if (_connected == value)
            {
                // nothing to do
                return;
            }

            // set the value
            _connected = value;

            // invoke the associated event
            OnConnectionStateChanged(new ConnectionStatusChangedEventArgs(value));
        }
    }

    #endregion

    #region ctors

    /// <summary>
    /// A parameterized constructor of the <see cref="RmqcCore"/> class.
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="logger"></param>
    [ActivatorUtilitiesConstructor]
    public RmqcCore(RmqcSettings settings, ILogger<RmqcCore>? logger = null)
    {
        // init global vars
        _settings = settings;
        _logger = logger;
        _connected = false;

        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);
    }

    /// <summary>
    /// A parameterized constructor of the <see cref="RmqcCore"/> class.
    /// For unit-tests only!
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="logger"></param>
    /// <param name="connectionFactory"></param>
    /// <param name="connection"></param>
    /// <param name="channel"></param>
    internal RmqcCore(RmqcSettings settings, ILogger<RmqcCore> logger,
        IConnectionFactory? connectionFactory, IConnection? connection, IModel? channel)
    {
        // init global vars
        _settings = settings;
        _logger = logger;
        _connectionFactory = connectionFactory;
        _connection = connection;
        _channel = channel;
        _connected = false;

        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);
    }

    #endregion

    #region public events

    /// <inheritdoc />
    public event EventHandler<ConnectCompletedEventArgs>? ConnectCompleted;

    /// <inheritdoc />
    public event EventHandler<DisconnectCompletedEventArgs>? DisconnectCompleted;

    /// <inheritdoc />
    public event EventHandler<WriteMsgCompletedEventArgs>? WriteMsgCompleted;

    /// <inheritdoc />
    public event EventHandler<ReadMsgCompletedEventArgs>? ReadMsgCompleted;

    /// <inheritdoc />
    public event EventHandler<AckMsgCompletedEventArgs>? AckMsgCompleted;

    /// <inheritdoc />
    public event EventHandler<NackMsgCompletedEventArgs>? NackMsgComplete;

    /// <inheritdoc />
    public event EventHandler<QueuedMsgsCompletedEventArgs>? QueuedMsgsCompleted;

    /// <inheritdoc />
    public event EventHandler<ConnectionStatusChangedEventArgs>? ConnectionStateChanged;

    /// <inheritdoc />
    public event EventHandler<ErrorOccurredEventArgs>? ErrorOccurred;

    #endregion

    #region public methods

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
            VirtualHost = Settings.VirtualHost
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

        // init performance measurement
        var sw = new Stopwatch();
        sw.Start();

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
            Disconnect(out _);

            // invoke associated event
            OnErrorOccurred(new ErrorOccurredEventArgs(exc));

            // set output 
            exception = exc;
            res = false;
        }

        // set connected state
        Connected = res;

        // end performance measurement
        sw.Stop();

        // invoke associated event
        OnConnectCompleted(new ConnectCompletedEventArgs(res, exception, ((int)sw.ElapsedMilliseconds).MillisecondsToTimeSpan()));

        // return the result
        return res;
    }

    /// <inheritdoc />
    public bool Disconnect(out Exception? exception)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // init performance measurement
        var sw = new Stopwatch();
        sw.Start();

        // init output
        var res = true;
        exception = null;

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

            // invoke associated event
            OnErrorOccurred(new ErrorOccurredEventArgs(exc));

            // set output 
            exception = exc;
            res = false;
        }

        // set connected state
        Connected = false;

        // end performance measurement
        sw.Stop();

        // invoke associated event
        OnDisconnectCompleted(new DisconnectCompletedEventArgs(res, exception, ((int)sw.ElapsedMilliseconds).MillisecondsToTimeSpan()));

        // return the result
        return res;
    }

    /// <inheritdoc />
    public bool WriteMsg(ref RmqcMessage message, out Exception? exception)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // init performance measurement
        var sw = new Stopwatch();
        sw.Start();

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

            // crate basic properties
            var props = _channel?.CreateBasicProperties();
            if (message.Headers is not null && props is not null)
            {
                props.Headers = message.Headers;
            } 
            
            // write message
            _channel?.BasicPublish(Settings.Exchange, message.RoutingKey, props, message.BodyArray ?? null);
            message.TimestampWhenSend = DateTime.Now;
        }
        catch (Exception exc)
        {
            // log exception
            _logger?.LogExcInFctCall(exc, GetType(), MethodBase.GetCurrentMethod(), exc.Message, LogLevel.Warning);

            // invoke associated event
            OnErrorOccurred(new ErrorOccurredEventArgs(exc));

            // set output 
            exception = exc;
            res = false;
        }

        // end performance measurement
        sw.Stop();

        // invoke associated event
        OnWriteMsgCompleted(new WriteMsgCompletedEventArgs(res, exception, ((int)sw.ElapsedMilliseconds).MillisecondsToTimeSpan()));

        // return the result
        return res;
    }

    /// <inheritdoc />
    public bool ReadMsg(out RmqcMessage? message, bool autoAck, out Exception? exception)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // init performance measurement
        var sw = new Stopwatch();
        sw.Start();

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
                message = new RmqcMessage(msg)
                {
                    TimestampWhenReceived = DateTime.Now
                };

                if (autoAck)
                {
                    message.TimestampWhenAcked = message.TimestampWhenReceived;
                }
            }
        }
        catch (Exception exc)
        {
            // log exception
            _logger?.LogExcInFctCall(exc, GetType(), MethodBase.GetCurrentMethod(), exc.Message, LogLevel.Warning);

            // invoke associated event
            OnErrorOccurred(new ErrorOccurredEventArgs(exc));

            // set output 
            exception = exc;
            res = false;
        }

        // end performance measurement
        sw.Stop();

        // invoke associated event
        OnReadMsgCompleted(new ReadMsgCompletedEventArgs(res, exception, ((int)sw.ElapsedMilliseconds).MillisecondsToTimeSpan(), message));

        // return the result
        return res;
    }

    /// <inheritdoc />
    public bool AckMsg(ref RmqcMessage message, out Exception? exception)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // init performance measurement
        var sw = new Stopwatch();
        sw.Start();

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
            message.TimestampWhenAcked = DateTime.Now;
        }
        catch (Exception exc)
        {
            // log exception
            _logger?.LogExcInFctCall(exc, GetType(), MethodBase.GetCurrentMethod(), exc.Message, LogLevel.Warning);

            // invoke associated event
            OnErrorOccurred(new ErrorOccurredEventArgs(exc));

            // set output 
            exception = exc;
            res = false;
        }

        // end performance measurement
        sw.Stop();

        // invoke associated event
        OnAckMsgCompleted(new AckMsgCompletedEventArgs(res, exception, ((int)sw.ElapsedMilliseconds).MillisecondsToTimeSpan(), message.DeliveryTag));

        // return the result
        return res;
    }

    /// <inheritdoc />
    public bool NackMsg(ref RmqcMessage message, bool requeue, out Exception? exception)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // init performance measurement
        var sw = new Stopwatch();
        sw.Start();

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
            message.TimestampWhenNacked = DateTime.Now;
            message.WasNackedWithRequeue = requeue;
        }
        catch (Exception exc)
        {
            // log exception
            _logger?.LogExcInFctCall(exc, GetType(), MethodBase.GetCurrentMethod(), exc.Message, LogLevel.Warning);

            // invoke associated event
            OnErrorOccurred(new ErrorOccurredEventArgs(exc));

            // set output 
            exception = exc;
            res = false;
        }

        // end performance measurement
        sw.Stop();

        // invoke associated event
        OnNackMsgComplete(new NackMsgCompletedEventArgs(res, exception, ((int)sw.ElapsedMilliseconds).MillisecondsToTimeSpan(), message.DeliveryTag));

        // return the result
        return res;
    }

    /// <inheritdoc />
    public bool WaitForWriteConfirm(TimeSpan timeout, out Exception? exception)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // init performance measurement
        var sw = new Stopwatch();
        sw.Start();

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

            // invoke associated event
            OnErrorOccurred(new ErrorOccurredEventArgs(exc));

            // set output 
            exception = exc;
            res = false;
        }

        // end performance measurement
        sw.Stop();

        // invoke associated event
        // OnConnectCompleted(new ConnectCompletedEventArgs(res, exception, ((int)sw.ElapsedMilliseconds).MillisecondsToTimeSpan()));

        // return the result
        return res;
    }

    /// <inheritdoc />
    public bool QueuedMsgs(out uint? amount, out Exception? exception)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // init performance measurement
        var sw = new Stopwatch();
        sw.Start();

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

            // invoke associated event
            OnErrorOccurred(new ErrorOccurredEventArgs(exc));

            // set output 
            exception = exc;
            res = false;
        }

        // end performance measurement
        sw.Stop();

        // invoke associated event
        OnQueuedMsgsCompleted(new QueuedMsgsCompletedEventArgs(res, exception, ((int)sw.ElapsedMilliseconds).MillisecondsToTimeSpan(), amount));

        // return the result
        return res;
    }

    #endregion
    
    #region private event invokation

    /// <summary>
    /// This method invokes the <see cref="ConnectCompleted"/> envent handlers.
    /// </summary>
    /// <param name="e"></param>
    private void OnConnectCompleted(ConnectCompletedEventArgs e)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // invoke event handlers
        ConnectCompleted?.Invoke(this, e);
    }

    /// <summary>
    /// This method invokes the <see cref="DisconnectCompleted"/> envent handlers.
    /// </summary>
    /// <param name="e"></param>
    private void OnDisconnectCompleted(DisconnectCompletedEventArgs e)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // invoke event handlers
        DisconnectCompleted?.Invoke(this, e);
    }

    /// <summary>
    /// This method invokes the <see cref="WriteMsgCompleted"/> envent handlers.
    /// </summary>
    /// <param name="e"></param>
    private void OnWriteMsgCompleted(WriteMsgCompletedEventArgs e)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // invoke event handlers
        WriteMsgCompleted?.Invoke(this, e);
    }

    /// <summary>
    /// This method invokes the <see cref="ReadMsgCompleted"/> envent handlers.
    /// </summary>
    /// <param name="e"></param>
    private void OnReadMsgCompleted(ReadMsgCompletedEventArgs e)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // invoke event handlers
        ReadMsgCompleted?.Invoke(this, e);
    }

    /// <summary>
    /// This method invokes the <see cref="AckMsgCompleted"/> envent handlers.
    /// </summary>
    /// <param name="e"></param>
    private void OnAckMsgCompleted(AckMsgCompletedEventArgs e)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // invoke event handlers
        AckMsgCompleted?.Invoke(this, e);
    }

    /// <summary>
    /// This method invokes the <see cref="NackMsgComplete"/> envent handlers.
    /// </summary>
    /// <param name="e"></param>
    private void OnNackMsgComplete(NackMsgCompletedEventArgs e)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // invoke event handlers
        NackMsgComplete?.Invoke(this, e);
    }

    /// <summary>
    /// This method invokes the <see cref="QueuedMsgsCompleted"/> envent handlers.
    /// </summary>
    /// <param name="e"></param>
    private void OnQueuedMsgsCompleted(QueuedMsgsCompletedEventArgs e)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // invoke event handlers
        QueuedMsgsCompleted?.Invoke(this, e);
    }

    /// <summary>
    /// This method invokes the <see cref="ConnectionStateChanged"/> envent handlers.
    /// </summary>
    /// <param name="e"></param>
    private void OnConnectionStateChanged(ConnectionStatusChangedEventArgs e)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // invoke event handlers
        ConnectionStateChanged?.Invoke(this, e);
    }

    /// <summary>
    /// This method invokes the <see cref="ErrorOccurred"/> envent handlers.
    /// </summary>
    /// <param name="e"></param>
    private void OnErrorOccurred(ErrorOccurredEventArgs e)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // invoke event handlers
        ErrorOccurred?.Invoke(this, e);
    }

    #endregion
}

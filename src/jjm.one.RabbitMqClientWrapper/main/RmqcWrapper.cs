using System;
using System.Diagnostics;
using System.Reflection;
using jjm.one.Microsoft.Extensions.Logging.Helpers;
using jjm.one.RabbitMqClientWrapper.main.core;
using jjm.one.RabbitMqClientWrapper.types;
using jjm.one.RabbitMqClientWrapper.types.events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace jjm.one.RabbitMqClientWrapper.main;

/// <summary>
/// This class implements the <see cref="IRmqcWrapper"/> interface for a RabbitMQ server.
/// </summary>
public class RmqcWrapper : IRmqcWrapper
{
    #region private members

    private readonly IRmqcCore _core;
    private readonly ILogger<RmqcWrapper>? _logger;

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

            return _core.Settings;
        }
        set
        {
            // log fct call
            _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            _core.Settings = value;
        }
    }

    /// <inheritdoc />
    public bool Connected
    {
        get
        {
            // log fct call
            _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            return _core.Connected;
        }
    }

    #endregion

    #region ctors

    /// <summary>
    /// A parameterized constructor of the <see cref="RmqcWrapper"/> class.
    /// </summary>
    /// <param name="core"></param>
    /// <param name="logger"></param>
    [ActivatorUtilitiesConstructor]
    public RmqcWrapper(IRmqcCore core, ILogger<RmqcWrapper>? logger)
    {
        _core = core;
        _logger = logger;

        // register event callbacks
        _core.ConnectCompleted += OnConnectCompleted;
        _core.DisconnectCompleted += OnDisconnectCompleted;
        _core.WriteMsgCompleted += OnWriteMsgCompleted;
        _core.ReadMsgCompleted += OnReadMsgCompleted;
        _core.AckMsgCompleted += OnAckMsgCompleted;
        _core.NAckMsgComplete += OnNAckMsgComplete;
        _core.QueuedMsgsCompleted += OnQueuedMsgsCompleted;
        _core.ConnectionStateChanged += OnConnectionStateChanged;
        _core.ErrorOccurred += OnErrorOccurred;

        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);
    }

    /// <summary>
    /// A parameterized constructor of the <see cref="RmqcWrapper"/> class.
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="logger"></param>
    public RmqcWrapper(RmqcSettings settings, 
        ILogger<RmqcWrapper>? logger = null)
    {
        _core = new RmqcCore(settings);
        _logger = logger;

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
    public event EventHandler<ReConnectCompletedEventArgs>? ReConnectCompleted;

    /// <inheritdoc />
    public event EventHandler<WriteMsgCompletedEventArgs>? WriteMsgCompleted;
    
    /// <inheritdoc />
    public event EventHandler<ReadMsgCompletedEventArgs>? ReadMsgCompleted;
    
    /// <inheritdoc />
    public event EventHandler<AckMsgCompletedEventArgs>? AckMsgCompleted;
    
    /// <inheritdoc />
    public event EventHandler<NackMsgCompletedEventArgs>? NAckMsgComplete;
    
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

        _core.Init();
    }

    /// <inheritdoc />
    public void DeInit()
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        _core.DeInit();
    }

    /// <inheritdoc />
    public bool Connect()
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return Connect(out _);
    }

    /// <inheritdoc />
    public bool Connect(out Exception? exception)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // call the core functions
        return _core.Connect(out exception);
    }

    /// <inheritdoc />
    public bool Disconnect()
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return Disconnect(out _);
    }
    
    /// <inheritdoc />
    public bool Disconnect(out Exception? exception)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // call the core functions
        return _core.Disconnect(out exception);
    }

    /// <inheritdoc />
    public bool ReConnect()
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return ReConnect(out _);
    }

    /// <inheritdoc />
    public bool ReConnect(out Exception? exception)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // init measure completion time
        var sw = new Stopwatch();
        sw.Start();

        // init output
        var res = true;

        // disconnect
        res = Disconnect(out exception);

        if (res)
        {
            // de-init
            DeInit();

            // init
            Init();

            // connect
            res = Connect(out exception);
        }

        // measure completion time
        sw.Stop();

        // invoke events
        OnReConnectCompleted(new ReConnectCompletedEventArgs(res, exception,
            TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds)));

        // return the result
        return res;
    }

    /// <inheritdoc />
    public bool WriteMsg(ref RmqcMessage message)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return WriteMsg(ref message, out _);
    }

    /// <inheritdoc />
    public bool WriteMsg(ref RmqcMessage message, out Exception? exception)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // call the core functions
        return _core.WriteMsg(ref message, out exception);
    }

    /// <inheritdoc />
    public bool ReadMsg(out RmqcMessage? message, bool autoAck)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return ReadMsg(out message, autoAck, out _);
    }

    /// <inheritdoc />
    public bool ReadMsg(out RmqcMessage? message, bool autoAck, out Exception? exception)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // call the core functions
        return _core.ReadMsg(out message, autoAck, out exception);
    }

    /// <inheritdoc />
    public bool AckMsg(ref RmqcMessage message)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return AckMsg(ref message, out _);
    }

    /// <inheritdoc />
    public bool AckMsg(ref RmqcMessage message, out Exception? exception)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);
        
        // call the core functions
        return _core.AckMsg(ref message, out exception);
    }

    /// <inheritdoc />
    public bool NAckMsg(ref RmqcMessage message, bool requeue)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);
        
        return NackMsg(ref message, requeue, out _);
    }

    /// <inheritdoc />
    public bool NackMsg(ref RmqcMessage message, bool requeue, out Exception? exception)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // call the core functions
        return _core.NackMsg(ref message, requeue, out exception);
    }

    /// <inheritdoc />
    public bool WaitForWriteConfirm(TimeSpan timeout)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return WaitForWriteConfirm(timeout, out _);
    }

    /// <inheritdoc />
    public bool WaitForWriteConfirm(TimeSpan timeout, out Exception? exception)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // return the result
        return _core.WaitForWriteConfirm(timeout, out exception);
    }

    /// <inheritdoc />
    public bool QueuedMsgs(out uint? amount)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return QueuedMsgs(out amount, out _);
    }

    /// <inheritdoc />
    public bool QueuedMsgs(out uint? amount, out Exception? exception)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // call the core functions
        return _core.QueuedMsgs(out amount, out exception);
    }

    #endregion

    #region private event callbacks

    /// <summary>
    /// This method invokes the <see cref="ConnectCompleted"/> envent handlers.
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private void OnConnectCompleted(object? o, ConnectCompletedEventArgs e)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // invoke event handlers
        ConnectCompleted?.Invoke(o, e);
    }

    /// <summary>
    /// This method invokes the <see cref="DisconnectCompleted"/> envent handlers.
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private void OnDisconnectCompleted(object? o, DisconnectCompletedEventArgs e)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // invoke event handlers
        DisconnectCompleted?.Invoke(o, e);
    }

    /// <summary>
    /// This method invokes the <see cref="WriteMsgCompleted"/> envent handlers.
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private void OnWriteMsgCompleted(object? o, WriteMsgCompletedEventArgs e)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // invoke event handlers
        WriteMsgCompleted?.Invoke(o, e);
    }

    /// <summary>
    /// This method invokes the <see cref="ReadMsgCompleted"/> envent handlers.
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private void OnReadMsgCompleted(object? o, ReadMsgCompletedEventArgs e)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // invoke event handlers
        ReadMsgCompleted?.Invoke(o, e);
    }

    /// <summary>
    /// This method invokes the <see cref="AckMsgCompleted"/> envent handlers.
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private void OnAckMsgCompleted(object? o, AckMsgCompletedEventArgs e)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // invoke event handlers
        AckMsgCompleted?.Invoke(o, e);
    }

    /// <summary>
    /// This method invokes the <see cref="NAckMsgComplete"/> envent handlers.
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private void OnNAckMsgComplete(object? o, NackMsgCompletedEventArgs e)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // invoke event handlers
        NAckMsgComplete?.Invoke(o, e);
    }

    /// <summary>
    /// This method invokes the <see cref="QueuedMsgsCompleted"/> envent handlers.
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private void OnQueuedMsgsCompleted(object? o, QueuedMsgsCompletedEventArgs e)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // invoke event handlers
        QueuedMsgsCompleted?.Invoke(o, e);
    }

    /// <summary>
    /// This method invokes the <see cref="ConnectionStateChanged"/> envent handlers.
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private void OnConnectionStateChanged(object? o, ConnectionStatusChangedEventArgs e)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // invoke event handlers
        ConnectionStateChanged?.Invoke(o, e);
    }

    /// <summary>
    /// This method invokes the <see cref="ErrorOccurred"/> envent handlers.
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private void OnErrorOccurred(object? o, ErrorOccurredEventArgs e)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        // invoke event handlers
        ErrorOccurred?.Invoke(o, e);
    }

    #endregion

    #region private event invokation

    /// <summary>
    /// This method invokes the <see cref="ReConnectCompleted"/> envent handlers.
    /// </summary>
    /// <param name="e"></param>
    private void OnReConnectCompleted(ReConnectCompletedEventArgs e)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);
        
        // invoke event handlers
        ReConnectCompleted?.Invoke(this, e);
    }

    #endregion
}

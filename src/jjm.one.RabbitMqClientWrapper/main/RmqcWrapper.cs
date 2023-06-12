using System;
using System.Diagnostics;
using System.Reflection;
using jjm.one.Microsoft.Extensions.Logging.Helpers;
using jjm.one.RabbitMqClientWrapper.main.core;
using jjm.one.RabbitMqClientWrapper.types;
using jjm.one.RabbitMqClientWrapper.types.events;
using jjm.one.RabbitMqClientWrapper.util;
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
    public Settings Settings
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
    /// A parameterised constructor of the <see cref="RmqcWrapper"/> class.
    /// </summary>
    /// <param name="core"></param>
    /// <param name="logger"></param>
    [ActivatorUtilitiesConstructor]
    public RmqcWrapper(IRmqcCore core, ILogger<RmqcWrapper>? logger)
    {
        _core = core;
        _logger = logger;

        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);
    }

    /// <summary>
    /// A parameterised constructor of the <see cref="RmqcWrapper"/> class.
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="logger"></param>
    public RmqcWrapper(Settings settings, 
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
        
        // init measure completion time
        var sw = new Stopwatch();
        sw.Start();

        // call the core functions
        var res = _core.Connect(out exception);

        // measure completion time
        sw.Stop();

        // invoke events
        OnConnectCompleted(new ConnectCompletedEventArgs(res, exception, 
            ((int)sw.ElapsedMilliseconds).MillisecondsToTimeSpan()));
        
        return res;
    }

    /// <inheritdoc />
    void IRmqcCore.Disconnect()
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        Disconnect(out _);
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

        // init measure completion time
        var sw = new Stopwatch();
        sw.Start();

        // init local vars
        exception = null;
        var res = true;

        try
        {
            // call the core functions
            _core.Disconnect();
        }
        catch (Exception exc)
        {
            // log exception
            _logger?.LogExcInFctCall(exc, GetType(), MethodBase.GetCurrentMethod(), exc.Message, LogLevel.Warning);

            
            exception = exc;
            res = false;
        }
        
        // measure completion time
        sw.Stop();

        // invoke events
        OnDisconnectCompleted(new DisconnectCompletedEventArgs(res, exception, 
            ((int)sw.ElapsedMilliseconds).MillisecondsToTimeSpan()));
        
        return res;
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

        // call the core functions
        Disconnect();
        var res = Connect(out exception);

        // measure completion time
        sw.Stop();

        // invoke events
        OnReConnectCompleted(new ReConnectCompletedEventArgs(res, exception, 
            ((int)sw.ElapsedMilliseconds).MillisecondsToTimeSpan()));
        
        return res;
    }

    /// <inheritdoc />
    public bool WriteMsg(Message message)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return WriteMsg(message, out _);
    }

    /// <inheritdoc />
    public bool WriteMsg(Message message, out Exception? exception)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return _core.WriteMsg(message, out exception);
    }

    /// <inheritdoc />
    public bool ReadMsg(out Message? message, bool autoAck)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return ReadMsg(out message, autoAck, out _);
    }

    /// <inheritdoc />
    public bool ReadMsg(out Message? message, bool autoAck, out Exception? exception)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return _core.ReadMsg(out message, autoAck, out exception);
    }

    /// <inheritdoc />
    public bool AckMsg(Message message)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return AckMsg(message, out _);
    }

    /// <inheritdoc />
    public bool AckMsg(Message message, out Exception? exception)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return _core.AckMsg(message, out exception);
    }

    /// <inheritdoc />
    public bool NackMsg(Message message, bool requeue)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return NackMsg(message, requeue, out _);
    }

    /// <inheritdoc />
    public bool NackMsg(Message message, bool requeue, out Exception? exception)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return _core.NackMsg(message, requeue, out exception);
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

        return _core.QueuedMsgs(out amount, out exception);
    }

    #endregion

    #region protected event invokation

    /// <summary>
    /// This method invokes the <see cref="ConnectCompleted"/> envent handlers.
    /// </summary>
    /// <param name="e"></param>
    protected virtual void OnConnectCompleted(ConnectCompletedEventArgs e)
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
    protected virtual void OnDisconnectCompleted(DisconnectCompletedEventArgs e)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);
        
        // invoke event handlers
        DisconnectCompleted?.Invoke(this, e);
    }

    /// <summary>
    /// This method invokes the <see cref="ReConnectCompleted"/> envent handlers.
    /// </summary>
    /// <param name="e"></param>
    protected virtual void OnReConnectCompleted(ReConnectCompletedEventArgs e)
    {
        // log fct call
        _logger?.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);
        
        // invoke event handlers
        ReConnectCompleted?.Invoke(this, e);
    }

    #endregion
}

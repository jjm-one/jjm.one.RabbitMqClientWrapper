using jjm.one.Microsoft.Extensions.Logging.Helpers;
using jjm.one.RabbitMqClientWrapper.main.core;
using jjm.one.RabbitMqClientWrapper.types;
using jjm.one.RabbitMqClientWrapper.types.di;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace jjm.one.RabbitMqClientWrapper.main;

/// <summary>
/// This class implements the <see cref="IRmqcWrapper"/> interface for a RabbitMQ server.
/// </summary>
public class RmqcWrapper : IRmqcWrapper
{
    #region private members

    private readonly IRmqcCore _core;
    private readonly ILogger<RmqcWrapper> _logger;
    private readonly bool _enableLogging;

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

            return _core.Settings;
        }
        set
        {
            // log fct call
            if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

            _core.Settings = value;
        }
    }

    /// <inheritdoc />
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

    /// <summary>
    /// A additional parameterised constructor of the <see cref="RmqcWrapper"/> class.
    /// </summary>
    /// <param name="core"></param>
    /// <param name="logger"></param>
    /// <param name="enableLogging"></param>
    [ActivatorUtilitiesConstructor]
    public RmqcWrapper(IRmqcCore core, ILogger<RmqcWrapper> logger,
        DiSimpleTypeWrappersEnableWrapperLogging? enableLogging = null)
    {
        _core = core;
        _logger = logger;
        _enableLogging = enableLogging?.EnableLogging ?? false;

        // log fct call
        if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);
    }

    /// <summary>
    /// A additional parameterised constructor of the <see cref="RmqcWrapper"/> class.
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="enableWrapperLogging"></param>
    /// <param name="enableCoreLogging"></param>
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

    /// <inheritdoc />
    public void Init()
    {
        // log fct call
        if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        _core.Init();
    }

    /// <inheritdoc />
    public void DeInit()
    {
        // log fct call
        if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        _core.DeInit();
    }

    /// <inheritdoc />
    public bool Connect()
    {
        // log fct call
        if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return Connect(out _);
    }

    /// <inheritdoc />
    public bool Connect(out Exception? exception)
    {
        // log fct call
        if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return _core.Connect(out exception);
    }

    /// <inheritdoc />
    public void Disconnect()
    {
        // log fct call
        if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        _core.Disconnect();
    }

    /// <inheritdoc />
    public bool ReConnect()
    {
        // log fct call
        if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return ReConnect(out _);
    }

    /// <inheritdoc />
    public bool ReConnect(out Exception? exception)
    {
        // log fct call
        if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        _core.Disconnect();
        return _core.Connect(out exception);
    }

    /// <inheritdoc />
    public bool WriteMsg(Message message)
    {
        // log fct call
        if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return WriteMsg(message, out _);
    }

    /// <inheritdoc />
    public bool WriteMsg(Message message, out Exception? exception)
    {
        // log fct call
        if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return _core.WriteMsg(message, out exception);
    }

    /// <inheritdoc />
    public bool ReadMsg(out Message? message, bool autoAck)
    {
        // log fct call
        if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return ReadMsg(out message, autoAck, out _);
    }

    /// <inheritdoc />
    public bool ReadMsg(out Message? message, bool autoAck, out Exception? exception)
    {
        // log fct call
        if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return _core.ReadMsg(out message, autoAck, out exception);
    }

    /// <inheritdoc />
    public bool AckMsg(Message message)
    {
        // log fct call
        if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return AckMsg(message, out _);
    }

    /// <inheritdoc />
    public bool AckMsg(Message message, out Exception? exception)
    {
        // log fct call
        if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return _core.AckMsg(message, out exception);
    }

    /// <inheritdoc />
    public bool NackMsg(Message message, bool requeue)
    {
        // log fct call
        if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return NackMsg(message, requeue, out _);
    }

    /// <inheritdoc />
    public bool NackMsg(Message message, bool requeue, out Exception? exception)
    {
        // log fct call
        if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return _core.NackMsg(message, requeue, out exception);
    }

    /// <inheritdoc />
    public bool WaitForWriteConfirm(TimeSpan timeout)
    {
        // log fct call
        if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return WaitForWriteConfirm(timeout, out _);
    }

    /// <inheritdoc />
    public bool WaitForWriteConfirm(TimeSpan timeout, out Exception? exception)
    {
        // log fct call
        if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return _core.WaitForWriteConfirm(timeout, out exception);
    }

    /// <inheritdoc />
    public bool QueuedMsgs(out uint? amount)
    {
        // log fct call
        if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return QueuedMsgs(out amount, out _);
    }

    /// <inheritdoc />
    public bool QueuedMsgs(out uint? amount, out Exception? exception)
    {
        // log fct call
        if (_enableLogging) _logger.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), LogLevel.Trace);

        return _core.QueuedMsgs(out amount, out exception);
    }

    #endregion
}

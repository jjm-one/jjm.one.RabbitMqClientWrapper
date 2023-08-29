using System;
using jjm.one.RabbitMqClientWrapper.main.core;
using jjm.one.RabbitMqClientWrapper.types;
using jjm.one.RabbitMqClientWrapper.types.events;

namespace jjm.one.RabbitMqClientWrapper.main;

/// <summary>
/// This interface defines the core and additional functionality to connect and communicate with a RabbitMQ server.
/// </summary>
public interface IRmqcWrapper : IRmqcCore
{
    #region public methods

    /// <summary>
    /// Connect to the RabbitMQ server.
    /// </summary>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool Connect();
    
    /// <summary>
    /// Disconnect from the RabbitMQ server.
    /// </summary>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool Disconnect();

    /// <summary>
    /// Disconnect and connects form the RabbitMQ server.
    /// </summary>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool ReConnect();
    
    /// <summary>
    /// Disconnect and connects form the RabbitMQ server.
    /// </summary>
    /// <param name="exception"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool ReConnect(out Exception? exception);

    /// <summary>
    /// Write a <see cref="RmqcMessage"/> to the RabbitMQ server
    /// </summary>
    /// <param name="message"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool WriteMsg(ref RmqcMessage message);

    /// <summary>
    /// Read a <see cref="RmqcMessage"/> to the RabbitMQ server.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="autoAck"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool ReadMsg(out RmqcMessage? message, bool autoAck);

    /// <summary>
    /// Ack a received <see cref="RmqcMessage"/>.
    /// </summary>
    /// <param name="message"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool AckMsg(ref RmqcMessage message);

    /// <summary>
    /// NAck a received <see cref="RmqcMessage"/>.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="requeue"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool NAckMsg(ref RmqcMessage message, bool requeue);

    /// <summary>
    /// Wait until the server confirms the written <see cref="RmqcMessage"/>.
    /// </summary>
    /// <param name="timeout"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool WaitForWriteConfirm(TimeSpan timeout);

    /// <summary>
    /// Get the amount of <see cref="RmqcMessage"/> queued in the RabbitMQ server queue which are ready get read.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool QueuedMsgs(out uint? amount);

    #endregion

    #region public events

    /// <summary>
    /// This events gets invoked when the re-connect function finishes.
    /// </summary>
    public event EventHandler<ReConnectCompletedEventArgs> ReConnectCompleted;

    #endregion
}

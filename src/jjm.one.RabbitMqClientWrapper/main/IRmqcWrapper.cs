using System;
using jjm.one.RabbitMqClientWrapper.main.core;
using jjm.one.RabbitMqClientWrapper.types;

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
    /// Write a <see cref="Message"/> to the RabbitMQ server
    /// </summary>
    /// <param name="message"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool WriteMsg(Message message);

    /// <summary>
    /// Read a <see cref="Message"/> to the RabbitMQ server.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="autoAck"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool ReadMsg(out Message? message, bool autoAck);

    /// <summary>
    /// Ack a received <see cref="Message"/>.
    /// </summary>
    /// <param name="message"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool AckMsg(Message message);

    /// <summary>
    /// Nack a received <see cref="Message"/>.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="requeue"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool NackMsg(Message message, bool requeue);

    /// <summary>
    /// Wait until the server confirms the written <see cref="Message"/>.
    /// </summary>
    /// <param name="timeout"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool WaitForWriteConfirm(TimeSpan timeout);

    /// <summary>
    /// Get the amount of <see cref="Message"/> stored in the RabbitMQ server queue which are ready get read.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool QueuedMsgs(out uint? amount);

    #endregion
}

using System;
using jjm.one.RabbitMqClientWrapper.types;

namespace jjm.one.RabbitMqClientWrapper.main.core;

/// <summary>
/// This interface defines the core functionality to connect and communicate with a RabbitMQ server.
/// </summary>
public interface IRmqcCore
{
    #region public members

    /// <summary>
    /// This object contains the settings for the RabbitMQ client.
    /// </summary>
    public Settings Settings { get; set; }
    
    /// <summary>
    /// This flag indicates whether the client is connected the the RabbitMQ server or not.
    /// </summary>
    public bool Connected { get; }

    #endregion

    #region public methods

    /// <summary>
    /// Init the connection to the RabbitMQ server.
    /// </summary>
    public void Init();
    
    /// <summary>
    /// De-init the connection to the RabbitMQ server.
    /// </summary>
    public void DeInit();

    /// <summary>
    /// Connect to the RabbitMQ server.
    /// </summary>
    /// <param name="exception"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool Connect(out Exception? exception);
    
    /// <summary>
    /// Disconnect form the RabbitMQ server.
    /// </summary>
    public void Disconnect();

    /// <summary>
    /// Write a <see cref="Message"/> to the RabbitMQ server
    /// </summary>
    /// <param name="message"></param>
    /// <param name="exception"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool WriteMsg(Message message, out Exception? exception);
    
    /// <summary>
    /// Read a <see cref="Message"/> to the RabbitMQ server.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="autoAck"></param>
    /// <param name="exception"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool ReadMsg(out Message? message, bool autoAck, out Exception? exception);

    /// <summary>
    /// Ack a received <see cref="Message"/>.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="exception"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool AckMsg(Message message, out Exception? exception);
    
    /// <summary>
    /// Nack a received <see cref="Message"/>.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="requeue"></param>
    /// <param name="exception"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool NackMsg(Message message, bool requeue, out Exception? exception);

    /// <summary>
    /// Wait until the server confirms the written <see cref="Message"/>.
    /// </summary>
    /// <param name="timeout"></param>
    /// <param name="exception"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool WaitForWriteConfirm(TimeSpan timeout, out Exception? exception);

    /// <summary>
    /// Get the amount of <see cref="Message"/> stored in the RabbitMQ server queue which are ready get read.
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="exception"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool QueuedMsgs(out uint? amount, out Exception? exception);

    #endregion
}
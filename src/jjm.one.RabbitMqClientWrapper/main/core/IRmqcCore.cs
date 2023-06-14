using System;
using jjm.one.RabbitMqClientWrapper.types;
using jjm.one.RabbitMqClientWrapper.types.events;

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
    public RmqcSettings Settings { get; set; }
    
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
    /// <param name="exception"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool Disconnect(out Exception? exception);

    /// <summary>
    /// Write a <see cref="RmqcMessage"/> to the RabbitMQ server
    /// </summary>
    /// <param name="message"></param>
    /// <param name="exception"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool WriteMsg(ref RmqcMessage message, out Exception? exception);
    
    /// <summary>
    /// Read a <see cref="RmqcMessage"/> to the RabbitMQ server.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="autoAck"></param>
    /// <param name="exception"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool ReadMsg(out RmqcMessage? message, bool autoAck, out Exception? exception);

    /// <summary>
    /// Ack a received <see cref="RmqcMessage"/>.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="exception"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool AckMsg(ref RmqcMessage message, out Exception? exception);
    
    /// <summary>
    /// Nack a received <see cref="RmqcMessage"/>.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="requeue"></param>
    /// <param name="exception"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool NackMsg(ref RmqcMessage message, bool requeue, out Exception? exception);

    /// <summary>
    /// Wait until the server confirms the written <see cref="RmqcMessage"/>.
    /// </summary>
    /// <param name="timeout"></param>
    /// <param name="exception"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool WaitForWriteConfirm(TimeSpan timeout, out Exception? exception);

    /// <summary>
    /// Get the amount of <see cref="RmqcMessage"/> stored in the RabbitMQ server queue which are ready get read.
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="exception"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool QueuedMsgs(out uint? amount, out Exception? exception);

    #endregion

    #region public events

    /// <summary>
    /// This events gets invoked when the connect function finishes.
    /// </summary>
    public event EventHandler<ConnectCompletedEventArgs> ConnectCompleted;

    /// <summary>
    /// This events gets invoked when the disconnect function finishes.
    /// </summary>
    public event EventHandler<DisconnectCompletedEventArgs> DisconnectCompleted;

    /// <summary>
    /// This events gets invoked when the write msg function finishes.
    /// </summary>
    public event EventHandler<WriteMsgCompletedEventArgs> WriteMsgCompleted;

    /// <summary>
    /// This events gets invoked when the read msg function finishes.
    /// </summary>
    public event EventHandler<ReadMsgCompletedEventArgs> ReadMsgCompleted;

    /// <summary>
    /// This events gets invoked when the ack msg function finishes.
    /// </summary>
    public event EventHandler<AckMsgCompletedEventArgs> AckMsgCompleted;

    /// <summary>
    /// This events gets invoked when the nack msg function finishes.
    /// </summary>
    public event EventHandler<NackMsgCompletedEventArgs> NackMsgComplete;

    /// <summary>
    /// This events gets invoked when the queued msg's function finishes.
    /// </summary>
    public event EventHandler<QueuedMsgsCompletedEventArgs> QueuedMsgsCompleted;

    /// <summary>
    /// This events gets invoked when the connection status changes.
    /// </summary>
    public event EventHandler<ConnectionStatusChangedEventArgs> ConnectionStateChanged;

    /// <summary>
    /// This events gets invoked when an error occurred.
    /// </summary>
    public event EventHandler<ErrorOccurredEventArgs> ErrorOccurred;

    #endregion
}
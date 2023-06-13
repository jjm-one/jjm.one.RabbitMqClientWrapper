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
    public new bool Disconnect();
    
    /// <summary>
    /// Disconnect from the RabbitMQ server.
    /// </summary>
    /// <param name="exception"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool Disconnect(out Exception? exception);
    
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
    /// Get the amount of <see cref="Message"/> queued in the RabbitMQ server queue which are ready get read.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
    public bool QueuedMsgs(out uint? amount);

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
    /// This events gets invoked when the re-connect function finishes.
    /// </summary>
    public event EventHandler<ReConnectCompletedEventArgs> ReConnectCompleted;
    
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

    #endregion
}

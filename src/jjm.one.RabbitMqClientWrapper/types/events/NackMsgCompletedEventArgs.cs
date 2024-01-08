using System;
using System.Diagnostics.CodeAnalysis;

namespace jjm.one.RabbitMqClientWrapper.types.events;

/// <summary>
///     This class represents the event args for a nack completed event.
/// </summary>
[ExcludeFromCodeCoverage]
public class NackMsgCompletedEventArgs : EventArgs
{
    #region ctor

    /// <summary>
    ///     The default constructor of the <see cref="NackMsgCompletedEventArgs" /> class.
    /// </summary>
    /// <param name="successful"></param>
    /// <param name="exception"></param>
    /// <param name="completionTime"></param>
    /// <param name="deliveryTag"></param>
    public NackMsgCompletedEventArgs(bool successful = false, Exception? exception = null,
        TimeSpan? completionTime = null, ulong? deliveryTag = null)
    {
        Successful = successful;
        Exception = exception;
        CompletionTime = completionTime;
        DeliveryTag = deliveryTag;
    }

    #endregion

    #region public members

    /// <summary>
    ///     The flag indicates whether sending the nack to the RabbitMQ server was successfully completed or not.
    /// </summary>
    public bool Successful { get; set; }

    /// <summary>
    ///     The exception which may have occurred during sending the nack to the RabbitMQ server.
    /// </summary>
    public Exception? Exception { get; set; }

    /// <summary>
    ///     The amount of time it took to send the nack to the RabbitMQ server.
    /// </summary>
    public TimeSpan? CompletionTime { get; set; }

    /// <summary>
    ///     The delivery tag of the nack msg.
    /// </summary>
    public ulong? DeliveryTag { get; set; }

    #endregion
}
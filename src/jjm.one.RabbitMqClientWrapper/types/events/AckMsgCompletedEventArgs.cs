using System;
using System.Diagnostics.CodeAnalysis;

namespace jjm.one.RabbitMqClientWrapper.types.events;

/// <summary>
/// This class represents the event args for a ack completed event.
/// </summary>
[ExcludeFromCodeCoverage]
public class AckMsgCompletedEventArgs
{
    #region public members

    /// <summary>
    /// The flag indicates whether sending the ack to the RabbitMQ server was successfully completed or not.
    /// </summary>
    public bool Successful { get; set; }
    
    /// <summary>
    /// The exception which may have occured during sending the ack to the RabbitMQ server.
    /// </summary>
    public Exception? Exception { get; set; }
    
    /// <summary>
    /// The amount of time it took to send the ack to the RabbitMQ server.
    /// </summary>
    public TimeSpan? CompletionTime { get; set; }

    /// <summary>
    /// The delivery tag of the ack msg.
    /// </summary>
    public ulong? DeliveryTag { get; set; }
    
    #endregion

    #region ctor

    /// <summary>
    /// The default constructor of the <see cref="AckMsgCompletedEventArgs"/> class.
    /// </summary>
    /// <param name="successful"></param>
    /// <param name="exception"></param>
    /// <param name="completionTime"></param>
    /// <param name="deliveryTag"></param>
    public AckMsgCompletedEventArgs(bool successful = false, Exception? exception = null,
        TimeSpan? completionTime = null, ulong? deliveryTag = null)
    {
        Successful = successful;
        Exception = exception;
        CompletionTime = completionTime;
        DeliveryTag = deliveryTag;
    }

    #endregion
}
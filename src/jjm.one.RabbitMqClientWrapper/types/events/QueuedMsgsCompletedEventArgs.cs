using System;
using System.Diagnostics.CodeAnalysis;

namespace jjm.one.RabbitMqClientWrapper.types.events;

/// <summary>
/// This class represents the event args for a queued msg's completed event.
/// </summary>
[ExcludeFromCodeCoverage]
public class QueuedMsgsCompletedEventArgs
{
    #region public members

    /// <summary>
    /// The flag indicates whether getting the amount of queued msg's from the RabbitMQ server was successfully completed or not.
    /// </summary>
    public bool Successful { get; set; }
    
    /// <summary>
    /// The exception which may have occured during getting the amount of queued msg's from the RabbitMQ server.
    /// </summary>
    public Exception? Exception { get; set; }
    
    /// <summary>
    /// The amount of time it took to get the amount of queued msg's from the RabbitMQ server.
    /// </summary>
    public TimeSpan? CompletionTime { get; set; }
    
    /// <summary>
    /// The amount of queued msg's in the queue.
    /// </summary>
    public uint? Amount { get; set; }

    #endregion

    #region ctor

    /// <summary>
    /// The default constructor of the <see cref="QueuedMsgsCompletedEventArgs"/> class.
    /// </summary>
    /// <param name="successful"></param>
    /// <param name="exception"></param>
    /// <param name="completionTime"></param>
    /// <param name="amount"></param>
    public QueuedMsgsCompletedEventArgs(bool successful = false, Exception? exception = null,
        TimeSpan? completionTime = null, uint? amount = null)
    {
        Successful = successful;
        Exception = exception;
        CompletionTime = completionTime;
        Amount = amount;
    }

    #endregion
}
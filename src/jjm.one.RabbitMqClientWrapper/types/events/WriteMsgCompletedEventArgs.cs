using System;
using System.Diagnostics.CodeAnalysis;

namespace jjm.one.RabbitMqClientWrapper.types.events;

/// <summary>
/// This class represents the event args for a write completed event.
/// </summary>
[ExcludeFromCodeCoverage]
public class WriteMsgCompletedEventArgs
{
    #region public members

    /// <summary>
    /// The flag indicates whether writing a message to the RabbitMQ server was successfully completed or not.
    /// </summary>
    public bool Successful { get; set; }
    
    /// <summary>
    /// The exception which may have occured during writing a message to the RabbitMQ server.
    /// </summary>
    public Exception? Exception { get; set; }
    
    /// <summary>
    /// The amount of time it took to write a message to the RabbitMQ server.
    /// </summary>
    public TimeSpan? CompletionTime { get; set; }

    #endregion

    #region ctor

    /// <summary>
    /// The default constructor of the <see cref="WriteMsgCompletedEventArgs"/> class.
    /// </summary>
    /// <param name="successful"></param>
    /// <param name="exception"></param>
    /// <param name="completionTime"></param>
    public WriteMsgCompletedEventArgs(bool successful = false, Exception? exception = null,
        TimeSpan? completionTime = null)
    {
        Successful = successful;
        Exception = exception;
        CompletionTime = completionTime;
    }

    #endregion
}
using System;
using System.Diagnostics.CodeAnalysis;

namespace jjm.one.RabbitMqClientWrapper.types.events;

/// <summary>
/// This class represents the event args for a read completed event.
/// </summary>
[ExcludeFromCodeCoverage]
public class ReadCompletedEventArgs
{
    #region public members

    /// <summary>
    /// The flag indicates whether reading a messages from the RabbitMQ server was successfully completed or not.
    /// </summary>
    public bool Successful { get; set; }
    
    /// <summary>
    /// The exception which may have occured during reading a messages from the RabbitMQ server.
    /// </summary>
    public Exception? Exception { get; set; }
    
    /// <summary>
    /// The amount of time it took to read a message from the RabbitMQ server.
    /// </summary>
    public TimeSpan? CompletionTime { get; set; }
    
    /// <summary>
    /// The read message.
    /// </summary>
    public Message? Message { get; set; }

    #endregion

    #region ctor

    /// <summary>
    /// The default constructor of the <see cref="ReadCompletedEventArgs"/> class.
    /// </summary>
    /// <param name="successful"></param>
    /// <param name="exception"></param>
    /// <param name="completionTime"></param>
    /// <param name="message"></param>
    public ReadCompletedEventArgs(bool successful = false, Exception? exception = null,
        TimeSpan? completionTime = null, Message? message = null)
    {
        Successful = successful;
        Exception = exception;
        CompletionTime = completionTime;
        Message = message;
    }

    #endregion
}
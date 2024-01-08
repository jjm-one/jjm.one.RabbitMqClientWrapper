using System;
using System.Diagnostics.CodeAnalysis;

namespace jjm.one.RabbitMqClientWrapper.types.events;

/// <summary>
///     This class represents the event args for a read completed event.
/// </summary>
[ExcludeFromCodeCoverage]
public class ReadMsgCompletedEventArgs : EventArgs
{
    #region ctor

    /// <summary>
    ///     The default constructor of the <see cref="ReadMsgCompletedEventArgs" /> class.
    /// </summary>
    /// <param name="successful"></param>
    /// <param name="exception"></param>
    /// <param name="completionTime"></param>
    /// <param name="message"></param>
    public ReadMsgCompletedEventArgs(bool successful = false, Exception? exception = null,
        TimeSpan? completionTime = null, RmqcMessage? message = null)
    {
        Successful = successful;
        Exception = exception;
        CompletionTime = completionTime;
        Message = message;
    }

    #endregion

    #region public members

    /// <summary>
    ///     The flag indicates whether reading a messages from the RabbitMQ server was successfully completed or not.
    /// </summary>
    public bool Successful { get; set; }

    /// <summary>
    ///     The exception which may have occurred during reading a messages from the RabbitMQ server.
    /// </summary>
    public Exception? Exception { get; set; }

    /// <summary>
    ///     The amount of time it took to read a message from the RabbitMQ server.
    /// </summary>
    public TimeSpan? CompletionTime { get; set; }

    /// <summary>
    ///     The read message.
    /// </summary>
    public RmqcMessage? Message { get; set; }

    #endregion
}
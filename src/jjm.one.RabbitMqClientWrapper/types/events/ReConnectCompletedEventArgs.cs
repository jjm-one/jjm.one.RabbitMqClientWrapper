using System;
using System.Diagnostics.CodeAnalysis;

namespace jjm.one.RabbitMqClientWrapper.types.events;

/// <summary>
///     This class represents the event args for a re-connection completed event.
/// </summary>
[ExcludeFromCodeCoverage]
public class ReConnectCompletedEventArgs : EventArgs
{
    #region ctor

    /// <summary>
    ///     The default constructor of the <see cref="ReConnectCompletedEventArgs" /> class.
    /// </summary>
    /// <param name="successful"></param>
    /// <param name="exception"></param>
    /// <param name="completionTime"></param>
    public ReConnectCompletedEventArgs(bool successful = false, Exception? exception = null,
        TimeSpan? completionTime = null)
    {
        Successful = successful;
        Exception = exception;
        CompletionTime = completionTime;
    }

    #endregion

    #region public members

    /// <summary>
    ///     The flag indicates whether re-connecting to the RabbitMQ server was successfully completed or not.
    /// </summary>
    public bool Successful { get; set; }

    /// <summary>
    ///     The exception which may have occurred during re-connecting to the RabbitMQ server.
    /// </summary>
    public Exception? Exception { get; set; }

    /// <summary>
    ///     The amount of time it took to re-connect to the RabbitMQ server.
    /// </summary>
    public TimeSpan? CompletionTime { get; set; }

    #endregion
}
using System;

namespace jjm.one.RabbitMqClientWrapper.types.events;

/// <summary>
/// This class represents the event args for a disconnection completed event.
/// </summary>
public class DisconnectCompletedEventArgs : EventArgs
{
    #region public members

    /// <summary>
    /// The flag indicates whether disconnecting from the RabbitMQ server was successfully completed or not.
    /// </summary>
    public bool Successful { get; set; }
    
    /// <summary>
    /// The exception which may have occured during disconnecting from the RabbitMQ server.
    /// </summary>
    public Exception? Exception { get; set; }
    
    /// <summary>
    /// The amount of time it took to disconnect from the RabbitMQ server.
    /// </summary>
    public TimeSpan? CompletionTime { get; set; }

    #endregion
    
    #region ctor

    /// <summary>
    /// The default constructor of the <see cref="DisconnectCompletedEventArgs"/> class.
    /// </summary>
    /// <param name="successful"></param>
    /// <param name="exception"></param>
    /// <param name="completionTime"></param>
    public DisconnectCompletedEventArgs(bool successful = false, Exception? exception = null,
        TimeSpan? completionTime = null)
    {
        Successful = successful;
        Exception = exception;
        CompletionTime = completionTime;
    }

    #endregion
}
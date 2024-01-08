using System;
using System.Diagnostics.CodeAnalysis;

namespace jjm.one.RabbitMqClientWrapper.types.events;

/// <summary>
///     This class represents the event args for a error occurred event.
/// </summary>
[ExcludeFromCodeCoverage]
public class ErrorOccurredEventArgs : EventArgs
{
    #region ctor

    /// <summary>
    ///     The default constructor of the <see cref="ErrorOccurredEventArgs" /> class.
    /// </summary>
    /// <param name="exception"></param>
    public ErrorOccurredEventArgs(Exception? exception = null)
    {
        Exception = exception;
    }

    #endregion

    #region public members

    /// <summary>
    ///     The exception which may have occurred.
    /// </summary>
    public Exception? Exception { get; set; }

    #endregion
}
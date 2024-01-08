using System;
using System.Diagnostics.CodeAnalysis;

namespace jjm.one.RabbitMqClientWrapper.types.events;

/// <summary>
///     This class represents the event args for a connection status changed event.
/// </summary>
[ExcludeFromCodeCoverage]
public class ConnectionStatusChangedEventArgs : EventArgs
{
    #region ctor

    /// <summary>
    ///     The default constructor of the <see cref="ConnectionStatusChangedEventArgs" /> class.
    /// </summary>
    /// <param name="status"></param>
    public ConnectionStatusChangedEventArgs(bool status = false)
    {
        Status = status;
    }

    #endregion

    #region public members

    /// <summary>
    ///     The flag indicates the current connection status.
    /// </summary>
    public bool Status { get; set; }

    #endregion
}
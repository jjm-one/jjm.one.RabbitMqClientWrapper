﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace jjm.one.RabbitMqClientWrapper.types.events;

/// <summary>
/// This class represents the event args for a message changed event.
/// </summary>
[ExcludeFromCodeCoverage]
public class MsgChangedEventArgs : EventArgs
{
    #region public members

    /// <summary>
    /// The list of the names of all changed members.
    /// </summary>
    public List<string> ChangedMembers { get; set; }

    #endregion

    #region ctor

    /// <summary>
    /// The default constructor of the <see cref="MsgChangedEventArgs"/> class.
    /// </summary>
    /// <param name="changedMembers"></param>
    public MsgChangedEventArgs(List<string>? changedMembers)
    {
        ChangedMembers = changedMembers ?? new List<string>();
    }

    #endregion
}
using System;
using jjm.one.RabbitMqClientWrapper.main.core;
using RabbitMQ.Client;

namespace jjm.one.RabbitMqClientWrapper.types.exceptions;

/// <summary>
/// This class represents an exception which gets thrown if the <see cref="IModel"/> in a <see cref="RmqcCore"/> object is null or not initialized.
/// </summary>
[Serializable]
public class NoChannelException : Exception
{
    #region ctors

    /// <summary>
    /// The default constructor of the <see cref="NoChannelException"/> class.
    /// </summary>
    public NoChannelException() :
        base($"The {nameof(IModel)} is null! " +
             $"Maybe the {nameof(RmqcCore)} was not initialized properly.")
    {
    }
    
    #endregion
}
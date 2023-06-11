using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using jjm.one.RabbitMqClientWrapper.main.core;
using RabbitMQ.Client;

namespace jjm.one.RabbitMqClientWrapper.types.exceptions;

/// <summary>
/// This class represents an exception which gets thrown if the <see cref="IConnection"/> in a <see cref="RmqcCore"/> object is null or not initialized.
/// </summary>
[Serializable]
public class NoConnectionException : Exception
{
    #region ctors

    /// <summary>
    /// The default constructor of the <see cref="NoConnectionException"/> class.
    /// </summary>
    public NoConnectionException() : 
        base($"The {nameof(IConnection)} is null! " +
             $"Maybe the {nameof(RmqcCore)} was not initialized properly.")
    {
    }
    
    /// <summary>
    /// <see cref="ISerializable"/> compliant constructor for the <see cref="NoConnectionException"/> class.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    [ExcludeFromCodeCoverage]
    protected NoConnectionException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    #endregion
}
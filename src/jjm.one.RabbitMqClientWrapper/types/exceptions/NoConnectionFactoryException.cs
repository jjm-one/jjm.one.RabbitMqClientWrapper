using System;
using System.Runtime.Serialization;
using jjm.one.RabbitMqClientWrapper.main.core;
using RabbitMQ.Client;

namespace jjm.one.RabbitMqClientWrapper.types.exceptions;

/// <summary>
/// This class represents an exception which gets thrown if the <see cref="IConnectionFactory"/> in a <see cref="RmqcCore"/> object is null or not initialized.
/// </summary>
[Serializable]
public class NoConnectionFactoryException : Exception
{
    #region ctors

    /// <summary>
    /// The default constructor of the <see cref="NoConnectionFactoryException"/> class.
    /// </summary>
    public NoConnectionFactoryException() : 
        base($"The {nameof(IConnectionFactory)} is null! " +
             $"Maybe the {nameof(RmqcCore)} was not initialized properly.")
    {
    }
    
    /// <summary>
    /// <see cref="ISerializable"/> compliant constructor for the <see cref="NoConnectionFactoryException"/> class.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    private NoConnectionFactoryException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    #endregion
}
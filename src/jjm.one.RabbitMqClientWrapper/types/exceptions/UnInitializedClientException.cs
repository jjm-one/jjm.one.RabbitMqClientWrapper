using System;

namespace jjm.one.RabbitMqClientWrapper.types.exceptions;

/// <summary>
///     This class represents an exception which gets thrown if the client is not initialized properly before performing an
///     action.
/// </summary>
[Serializable]
public class UnInitializedClientException : Exception
{
    #region public members

    /// <summary>
    ///     The name of the operation which causes the exception.
    /// </summary>
    public string? Operation { get; set; }

    #endregion

    #region ctors

    /// <summary>
    ///     The default constructor of the <see cref="UnInitializedClientException" /> class.
    /// </summary>
    public UnInitializedClientException() :
        base("Client must be initialized and connected to perform this operation!")
    {
    }

    /// <summary>
    ///     A parameterized constructor of the <see cref="UnInitializedClientException" /> class.
    /// </summary>
    public UnInitializedClientException(string operation) :
        base($"Client must be initialized and connected to perform the {operation} operation!")
    {
        Operation = operation;
    }

    #endregion
}
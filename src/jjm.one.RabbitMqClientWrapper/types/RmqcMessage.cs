using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using RabbitMQ.Client;

namespace jjm.one.RabbitMqClientWrapper.types;

/// <summary>
/// This class represents a message which gets send an received to or from the RabbitMQ server.
/// </summary>
public class RmqcMessage
{
    #region private members

    private BasicGetResult? _rawBasicGetResult;

    #endregion

    #region internal members

    /// <summary>
    /// The raw <see cref="BasicGetResult"/> contained in this <see cref="RmqcMessage"/> object.
    /// </summary>
    internal BasicGetResult? RawBasicGetResult => _rawBasicGetResult;

    #endregion

    #region public members

    /// <summary>
    /// The delivery tag of this message.
    /// </summary>
    public ulong DeliveryTag => _rawBasicGetResult?.DeliveryTag ?? 0;

    /// <summary>
    /// The redelivered flag of this message.
    /// </summary>
    public bool Redelivered => _rawBasicGetResult?.Redelivered ?? false;

    /// <summary>
    /// The exchange to which the message was published to.
    /// </summary>
    public string? Exchange => _rawBasicGetResult?.Exchange ?? string.Empty;

    /// <summary>
    /// The routing key of this message.
    /// </summary>
    public string RoutingKey
    {
        get => _rawBasicGetResult is not null ? _rawBasicGetResult.RoutingKey : string.Empty;
        set
        {
            if (_rawBasicGetResult is null)
            {
                _rawBasicGetResult = new BasicGetResult(
                    deliveryTag: 0,
                    redelivered: false,
                    exchange: string.Empty,
                    routingKey: value,
                    messageCount: 0,
                    basicProperties: null,
                    body: null
                );
            }
            else
            {
                _rawBasicGetResult = new BasicGetResult(
                    deliveryTag: 0,
                    redelivered: false,
                    exchange: string.Empty,
                    routingKey: value,
                    messageCount: 0,
                    basicProperties: _rawBasicGetResult.BasicProperties,
                    body: _rawBasicGetResult.Body
                );
            }

            // reset additional fields
            TimestampWhenReceived = null;
            TimestampWhenSend = null;
            TimestampWhenAcked = null;
            TimestampWhenNacked = null;
            WasNackedWithRequeue = false;
            WasModified = true;
            WasSaved = false;
        }
    }

    /// <summary>
    /// The basic properties of the message.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public IBasicProperties? BasicProperties
    {
        get => _rawBasicGetResult?.BasicProperties;
        set
        {
            if (_rawBasicGetResult is null)
            {
                _rawBasicGetResult = new BasicGetResult(
                    deliveryTag: 0,
                    redelivered: false,
                    exchange: string.Empty,
                    routingKey: string.Empty,
                    messageCount: 0,
                    basicProperties: value,
                    body: null
                );
            }
            else
            {
                _rawBasicGetResult = new BasicGetResult(
                    deliveryTag: 0,
                    redelivered: false,
                    exchange: string.Empty,
                    routingKey: _rawBasicGetResult.RoutingKey,
                    messageCount: 0,
                    basicProperties: value,
                    body: _rawBasicGetResult.Body
                );
            }

            // reset additional fields
            TimestampWhenReceived = null;
            TimestampWhenSend = null;
            TimestampWhenAcked = null;
            TimestampWhenNacked = null;
            WasNackedWithRequeue = false;
            WasModified = true;
            WasSaved = false;
        }
    }

    /// <summary>
    /// The body containing the payload of the message. 
    /// </summary>
    public ReadOnlyMemory<byte>? Body
    {
        get => _rawBasicGetResult?.Body;
        set
        {
            if (_rawBasicGetResult is null)
            {
                if (value != null)
                {
                    _rawBasicGetResult = new BasicGetResult(
                        deliveryTag: 0,
                        redelivered: false,
                        exchange: string.Empty,
                        routingKey: string.Empty,
                        messageCount: 0,
                        basicProperties: null,
                        body: (ReadOnlyMemory<byte>)value
                    ); 
                }
                else
                {
                    _rawBasicGetResult = new BasicGetResult(
                        deliveryTag: 0,
                        redelivered: false,
                        exchange: string.Empty,
                        routingKey: string.Empty,
                        messageCount: 0,
                        basicProperties: null,
                        body: null
                    );
                }
                    
            }
            else
            {
                if (value != null)
                {
                    _rawBasicGetResult = new BasicGetResult(
                        deliveryTag: 0,
                        redelivered: false,
                        exchange: string.Empty,
                        routingKey: _rawBasicGetResult.RoutingKey,
                        messageCount: 0,
                        basicProperties: _rawBasicGetResult.BasicProperties,
                        body: (ReadOnlyMemory<byte>)value
                    );
                }
                else
                {
                    _rawBasicGetResult = new BasicGetResult(
                        deliveryTag: 0,
                        redelivered: false,
                        exchange: string.Empty,
                        routingKey: _rawBasicGetResult.RoutingKey,
                        messageCount: 0,
                        basicProperties: _rawBasicGetResult.BasicProperties,
                        body: null
                    );
                }
            }

            // reset additional fields
            TimestampWhenReceived = null;
            TimestampWhenSend = null;
            TimestampWhenAcked = null;
            TimestampWhenNacked = null;
            WasNackedWithRequeue = false;
            WasModified = true;
            WasSaved = false;
        }
    }

    /// <summary>
    /// This flag indicates whether the message was received via the client or not.
    /// </summary>
    public bool WasReceived => TimestampWhenReceived.HasValue;

    /// <summary>
    /// The Timestamp when the message was received.
    /// </summary>
    public DateTime? TimestampWhenReceived { get; internal set; }

    /// <summary>
    /// This flag indicates whether the message was send via the client or not.
    /// </summary>
    public bool WasSend => TimestampWhenSend.HasValue;

    /// <summary>
    /// The Timestamp when the message was send.
    /// </summary>
    public DateTime? TimestampWhenSend { get; internal set; }

    /// <summary>
    /// This flag indicates whether the message was acked via the client or not.
    /// </summary>
    public bool WasAcked => TimestampWhenAcked.HasValue;

    /// <summary>
    /// The Timestamp when the message was send.
    /// </summary>
    public DateTime? TimestampWhenAcked { get; internal set; }

    /// <summary>
    /// This flag indicates whether the message was nacked via the client or not.
    /// </summary>
    public bool WasNacked => TimestampWhenNacked.HasValue;

    /// <summary>
    /// This flag indicates whether the message was nacked  with or without requeue via the client or not.
    /// </summary>
    public bool WasNackedWithRequeue { get; internal set; }

    /// <summary>
    /// The Timestamp when the message was send.
    /// </summary>
    public DateTime? TimestampWhenNacked { get; internal set; }

    /// <summary>
    /// This flag indicates whether the message was modified.
    /// </summary>
    public bool WasModified { get; internal set; }

    /// <summary>
    /// This flag indicates whether the message was saved. (must be set by user!)
    /// </summary>
    public bool WasSaved { get; set; }

    #endregion

    #region ctors

    /// <summary>
    /// The default constructor of the <see cref="RmqcMessage"/> class.
    /// </summary>
    public RmqcMessage()
    {
    }

    /// <summary>
    /// The additional parameterized constructor of the <see cref="RmqcMessage"/> class.
    /// </summary>
    /// <param name="rawMessage"></param>
    public RmqcMessage(BasicGetResult? rawMessage)
    {
        _rawBasicGetResult = rawMessage;
    }

    #endregion
}

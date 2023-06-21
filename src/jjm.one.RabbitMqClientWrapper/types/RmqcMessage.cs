using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using RabbitMQ.Client;

namespace jjm.one.RabbitMqClientWrapper.types;

/// <summary>
/// This class represents a message which gets send an received to or from the RabbitMQ server.
/// </summary>
[Serializable]
public class RmqcMessage
{
    #region private members

    private ulong? _deliveryTag;
    private bool? _redelivered;
    private string? _exchange;
    private string? _routingKey;
    private uint? _messageCount;
    private IBasicProperties? _basicProperties;
    private IDictionary<string, object>? _headers;
    private byte[]? _body;

    #endregion

    #region public members

    /// <summary>
    /// The delivery tag of this message.
    /// </summary>
    public ulong DeliveryTag => _deliveryTag ?? 0;

    /// <summary>
    /// The redelivered flag of this message.
    /// </summary>
    public bool Redelivered => _redelivered ?? false;

    /// <summary>
    /// The exchange to which the message was published to.
    /// </summary>
    public string Exchange => _exchange ?? string.Empty;

    /// <summary>
    /// The number of message in the queue.
    /// </summary>
    public uint MessageCount => _messageCount ?? 0;
    
    /// <summary>
    /// The routing key of this message.
    /// </summary>
    public string RoutingKey
    {
        get => _routingKey ?? string.Empty;
        set
        {
            if (_routingKey == value)
            {
                return;
            }
            
            // set routing key
            _routingKey = value;

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
        get => _basicProperties;
        set
        {
            if (_basicProperties == value)
            {
                return;
            }

            // set basic properties
            _basicProperties = value;

            // copy headers
            if (BasicProperties is not null)
            {
                Headers = BasicProperties.Headers;
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
    /// The header entries of the message.
    /// </summary>
    public IDictionary<string, object>? Headers
    {
        get => _headers;
        set
        {
            if (_headers == value)
            {
                return;
            }
            
            // set headers
            _headers = value;

            // copy headers
            if (BasicProperties is not null && Headers is not null)
            {
                BasicProperties.Headers = Headers;
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
    public byte[]? BodyArray
    {
        get => _body;
        set
        {
            if (_body == value)
            {
                return;
            }
            
            // set body
            _body = value;

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
    /// The body containing the payload of the message.‚
    /// </summary>
    public string BodyString
    {
        get => Encoding.UTF8.GetString(_body ?? Array.Empty<byte>());
        set => BodyArray = Encoding.UTF8.GetBytes(value);
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
    public bool WasSaved
    {
        get => WasSaved;
        set
        {
            if (value is true)
            {
                WasModified = false;
            }
        }

    }

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
        _deliveryTag = rawMessage?.DeliveryTag;
        _redelivered = rawMessage?.Redelivered;
        _exchange = rawMessage?.Exchange;
        _routingKey = rawMessage?.RoutingKey;
        _messageCount = rawMessage?.MessageCount;
        _basicProperties = rawMessage?.BasicProperties;
        _headers = rawMessage?.BasicProperties?.Headers;
        _body = rawMessage?.Body.ToArray();
    }

    #endregion

    #region public methods

    

    #endregion
}

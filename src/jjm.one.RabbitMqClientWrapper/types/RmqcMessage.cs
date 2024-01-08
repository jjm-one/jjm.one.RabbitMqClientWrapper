using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using jjm.one.RabbitMqClientWrapper.types.events;
using RabbitMQ.Client;

namespace jjm.one.RabbitMqClientWrapper.types;

/// <summary>
///     This class represents a message which gets send an received to or from the RabbitMQ server.
/// </summary>
[Serializable]
public class RmqcMessage
{
    #region public events

    /// <summary>
    ///     This events gets invoked when the message got changed.
    /// </summary>
    public event EventHandler<MsgChangedEventArgs>? Changed;

    #endregion

    #region private event invokation

    /// <summary>
    ///     This method invokes the <see cref="Changed" /> envent handlers.
    /// </summary>
    /// <param name="changedMembers"></param>
    private void OnChanged(List<string> changedMembers)
    {
        // invoke event handlers
        Changed?.Invoke(this, new MsgChangedEventArgs(changedMembers));
    }

    #endregion

    #region private members

    private readonly ulong? _deliveryTag;
    private readonly bool? _redelivered;
    private readonly string? _exchange;
    private string? _routingKey;
    private readonly uint? _messageCount;
    private IBasicProperties? _basicProperties;
    private IDictionary<string, object>? _headers;
    private byte[]? _body;
    private bool _wasModified;
    private bool _wasSaved;

    #endregion

    #region public members

    /// <summary>
    ///     The delivery tag of this message.
    /// </summary>
    public ulong DeliveryTag => _deliveryTag ?? 0;

    /// <summary>
    ///     The redelivered flag of this message.
    /// </summary>
    public bool Redelivered => _redelivered ?? false;

    /// <summary>
    ///     The exchange to which the message was published to.
    /// </summary>
    public string Exchange => _exchange ?? string.Empty;

    /// <summary>
    ///     The number of message in the queue.
    /// </summary>
    public uint MessageCount => _messageCount ?? 0;

    /// <summary>
    ///     The routing key of this message.
    /// </summary>
    public string RoutingKey
    {
        get => _routingKey ?? string.Empty;
        set
        {
            if (_routingKey == value) return;

            // set routing key
            _routingKey = value;

            // reset additional fields
            TimestampWhenReceived = null;
            TimestampWhenSend = null;
            TimestampWhenAcked = null;
            TimestampWhenNAcked = null;
            WasNAckedWithRequeue = false;
            WasModified = true;
        }
    }

    /// <summary>
    ///     The basic properties of the message.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public IBasicProperties? BasicProperties
    {
        get => _basicProperties;
        set
        {
            if (_basicProperties == value) return;

            // set basic properties
            _basicProperties = value;

            // copy headers
            if (BasicProperties is not null) Headers = BasicProperties.Headers;

            // reset additional fields
            TimestampWhenReceived = null;
            TimestampWhenSend = null;
            TimestampWhenAcked = null;
            TimestampWhenNAcked = null;
            WasNAckedWithRequeue = false;
            WasModified = true;
        }
    }

    /// <summary>
    ///     The header entries of the message.
    /// </summary>
    public IDictionary<string, object>? Headers
    {
        get => _headers;
        set
        {
            if (_headers == value) return;

            // set headers
            _headers = value;

            // copy headers
            if (BasicProperties is not null && Headers is not null) BasicProperties.Headers = Headers;

            // reset additional fields
            TimestampWhenReceived = null;
            TimestampWhenSend = null;
            TimestampWhenAcked = null;
            TimestampWhenNAcked = null;
            WasNAckedWithRequeue = false;
            WasModified = true;
        }
    }

    /// <summary>
    ///     The body containing the payload of the message.
    /// </summary>
    public byte[]? BodyArray
    {
        get => _body;
        set
        {
            if (_body == value) return;

            // set body
            _body = value;

            // reset additional fields
            TimestampWhenReceived = null;
            TimestampWhenSend = null;
            TimestampWhenAcked = null;
            TimestampWhenNAcked = null;
            WasNAckedWithRequeue = false;
            WasModified = true;
        }
    }

    /// <summary>
    ///     The body containing the payload of the message.‚
    /// </summary>
    public string BodyString
    {
        get => Encoding.UTF8.GetString(_body ?? Array.Empty<byte>());
        set => BodyArray = Encoding.UTF8.GetBytes(value);
    }

    /// <summary>
    ///     This flag indicates whether the message was received via the client or not.
    /// </summary>
    public bool WasReceived => TimestampWhenReceived.HasValue;

    /// <summary>
    ///     The Timestamp when the message was received.
    /// </summary>
    public DateTime? TimestampWhenReceived { get; internal set; }

    /// <summary>
    ///     This flag indicates whether the message was send via the client or not.
    /// </summary>
    public bool WasSend => TimestampWhenSend.HasValue;

    /// <summary>
    ///     The Timestamp when the message was send.
    /// </summary>
    public DateTime? TimestampWhenSend { get; internal set; }

    /// <summary>
    ///     This flag indicates whether the message was acked via the client or not.
    /// </summary>
    public bool WasAcked => TimestampWhenAcked.HasValue;

    /// <summary>
    ///     The Timestamp when the message was send.
    /// </summary>
    public DateTime? TimestampWhenAcked { get; internal set; }

    /// <summary>
    ///     This flag indicates whether the message was nacked via the client or not.
    /// </summary>
    public bool WasNAcked => TimestampWhenNAcked.HasValue;

    /// <summary>
    ///     This flag indicates whether the message was nacked  with or without requeue via the client or not.
    /// </summary>
    public bool WasNAckedWithRequeue { get; internal set; }

    /// <summary>
    ///     The Timestamp when the message was send.
    /// </summary>
    public DateTime? TimestampWhenNAcked { get; internal set; }

    /// <summary>
    ///     This flag indicates whether the message was modified.
    /// </summary>
    public bool WasModified
    {
        get => _wasModified;
        internal set
        {
            if (value) WasSaved = false;

            _wasModified = value;

            OnChanged(new List<string> { "This information is currently not supported!" });
        }
    }

    /// <summary>
    ///     This flag indicates whether the message was saved. (must be set by user!)
    /// </summary>
    public bool WasSaved
    {
        get => _wasSaved;
        set
        {
            if (value) WasModified = false;

            _wasSaved = value;
        }
    }

    #endregion

    #region ctors

    /// <summary>
    ///     The default constructor of the <see cref="RmqcMessage" /> class.
    /// </summary>
    public RmqcMessage()
    {
    }

    /// <summary>
    ///     The additional parameterized constructor of the <see cref="RmqcMessage" /> class.
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
        //TimestampWhenReceived = rawMessage.BasicProperties.Timestamp.UnixTime @ToDo: fix timestamp
        _wasModified = false;
        _wasSaved = false;
    }

    #endregion
}
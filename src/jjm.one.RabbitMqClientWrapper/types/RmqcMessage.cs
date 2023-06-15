using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text;
using RabbitMQ.Client;

namespace jjm.one.RabbitMqClientWrapper.types;

/// <summary>
/// This class represents a message which gets send an received to or from the RabbitMQ server.
/// </summary>
[Serializable]
public class RmqcMessage : ISerializable
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

    /// <summary>
    /// The special constructor of the <see cref="RmqcMessage"/> class is used to deserialize values.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    public RmqcMessage(SerializationInfo info, StreamingContext context)
    {
        // Reset the property value using the GetValue method.
        
        // this
        TimestampWhenReceived = (DateTime?)info.GetValue("this.TimestampWhenReceived", typeof(DateTime?));
        TimestampWhenSend = (DateTime?)info.GetValue("this.TimestampWhenSend", typeof(DateTime?));
        TimestampWhenAcked = (DateTime?)info.GetValue("this.TimestampWhenAcked", typeof(DateTime?));
        TimestampWhenNacked = (DateTime?)info.GetValue("this.TimestampWhenNacked", typeof(DateTime?));
        WasNackedWithRequeue = (bool?)info.GetValue("this.WasNackedWithRequeue", typeof(bool?)) ?? false;
        WasModified = (bool?)info.GetValue("this.WasModified", typeof(bool?)) ?? false;
        WasSaved = (bool?)info.GetValue("this.WasSaved", typeof(bool?)) ?? false;
        
        // BasicGetResult

        _rawBasicGetResult = new BasicGetResult(
            deliveryTag: (ulong?)info.GetValue("this.BasicGetResult.DeliveryTag", typeof(ulong?)) ?? 0,
            routingKey: (string)info.GetValue("this.BasicGetResult.RoutingKey", typeof(string)),
            redelivered: (bool?)info.GetValue("this.BasicGetResult.Redelivered", typeof(bool?)) ?? false,
            exchange: (string)info.GetValue("this.BasicGetResult.Exchange", typeof(string)),
            messageCount: (uint?)info.GetValue("this.BasicGetResult.MessageCount", typeof(uint?)) ?? 0,
            basicProperties: (IBasicProperties?)info.GetValue("this.BasicGetResult.BasicProperties", typeof(IBasicProperties)),
            body: new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes((string)info.GetValue("this.BasicGetResult.Body", typeof(string))))
        );
    }
    
    #endregion

    #region ISerializable impl.

    /// <inheritdoc />
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        // Use the AddValue method to specify serialized values.
        
        // this
        info.AddValue("this.TimestampWhenReceived", TimestampWhenReceived ?? null, typeof(DateTime?));
        info.AddValue("this.TimestampWhenSend", TimestampWhenSend ?? null, typeof(DateTime?));
        info.AddValue("this.TimestampWhenAcked", TimestampWhenAcked ?? null, typeof(DateTime?));
        info.AddValue("this.TimestampWhenNacked", TimestampWhenNacked ?? null, typeof(DateTime?));
        info.AddValue("this.WasNackedWithRequeue", WasNackedWithRequeue, typeof(bool));
        info.AddValue("this.WasModified", WasModified, typeof(bool));
        info.AddValue("this.WasSaved", WasSaved, typeof(bool));

        // BasicGetResult 
        info.AddValue("this.BasicGetResult.DeliveryTag", _rawBasicGetResult?.DeliveryTag ?? null, typeof(ulong?));
        info.AddValue("this.BasicGetResult.RoutingKey", _rawBasicGetResult?.RoutingKey ?? null, typeof(string));
        info.AddValue("this.BasicGetResult.Redelivered", _rawBasicGetResult?.Redelivered ?? null, typeof(bool?));
        info.AddValue("this.BasicGetResult.Exchange", _rawBasicGetResult?.Exchange ?? null, typeof(string));
        info.AddValue("this.BasicGetResult.MessageCount", _rawBasicGetResult?.MessageCount ?? null, typeof(uint?));
        info.AddValue("this.BasicGetResult.BasicProperties", _rawBasicGetResult?.BasicProperties ?? null, typeof(IBasicProperties));
        info.AddValue("this.BasicGetResult.Body", Encoding.UTF8.GetString(_rawBasicGetResult?.Body.ToArray() ?? Array.Empty<byte>()), typeof(string));
        
        // BasicProperties= Encoding.UTF8.GetString(_message.Body.Value.ToArray())
        /*
        info.AddValue("BasicProperties.Expiration", _rawBasicGetResult?.BasicProperties.Expiration ?? null, typeof(string));
        info.AddValue("BasicProperties.Persistent", _rawBasicGetResult?.BasicProperties.Persistent ?? null, typeof(bool?));
        info.AddValue("BasicProperties.Type", _rawBasicGetResult?.BasicProperties.Type ?? null, typeof(string));
        info.AddValue("BasicProperties.Priority", _rawBasicGetResult?.BasicProperties.Priority ?? null, typeof(byte?));
        info.AddValue("BasicProperties.Timestamp", _rawBasicGetResult?.BasicProperties.Timestamp ?? null, typeof(AmqpTimestamp?));
        info.AddValue("BasicProperties.AppId", _rawBasicGetResult?.BasicProperties.AppId ?? null, typeof(string));
        info.AddValue("BasicProperties.ClusterId", _rawBasicGetResult?.BasicProperties.ClusterId ?? null, typeof(string));
        info.AddValue("BasicProperties.ContentEncoding", _rawBasicGetResult?.BasicProperties.ContentEncoding ?? null, typeof(string));
        info.AddValue("BasicProperties.ContentType", _rawBasicGetResult?.BasicProperties.ContentType ?? null, typeof(string));
        info.AddValue("BasicProperties.CorrelationId", _rawBasicGetResult?.BasicProperties.CorrelationId ?? null, typeof(string));
        info.AddValue("BasicProperties.DeliveryMode", _rawBasicGetResult?.BasicProperties.DeliveryMode ?? null, typeof(byte?));
        info.AddValue("BasicProperties.MessageId", _rawBasicGetResult?.BasicProperties.MessageId ?? null, typeof(string));
        info.AddValue("BasicProperties.ReplyTo", _rawBasicGetResult?.BasicProperties.ReplyTo ?? null, typeof(string));
        info.AddValue("BasicProperties.UserId", _rawBasicGetResult?.BasicProperties.UserId ?? null, typeof(string));
        info.AddValue("BasicProperties.ReplyToAddress", _rawBasicGetResult?.BasicProperties.ReplyToAddress ?? null, typeof(PublicationAddress));
        info.AddValue("BasicProperties.ProtocolClassId", _rawBasicGetResult?.BasicProperties.ProtocolClassId ?? null, typeof(ushort?));
        info.AddValue("BasicProperties.", _rawBasicGetResult?.BasicProperties. ?? null, typeof(?));
        info.AddValue("BasicProperties.", _rawBasicGetResult?.BasicProperties. ?? null, typeof(?));
        info.AddValue("BasicProperties.", _rawBasicGetResult?.BasicProperties. ?? null, typeof(?));
        info.AddValue("BasicProperties.", _rawBasicGetResult?.BasicProperties. ?? null, typeof(?));
        info.AddValue("BasicProperties.", _rawBasicGetResult?.BasicProperties. ?? null, typeof(?));
        */
        
    }

    #endregion
}

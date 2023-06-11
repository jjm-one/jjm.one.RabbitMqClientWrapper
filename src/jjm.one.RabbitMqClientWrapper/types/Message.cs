using System;
using RabbitMQ.Client;

namespace jjm.one.RabbitMqClientWrapper.types;

/// <summary>
/// This class represents a message which gets send an received to or from the RabbitMQ server.
/// </summary>
public class Message
{
    #region private members

    private BasicGetResult? _rawBasicGetResult;

    #endregion

    #region public members

    /// <summary>
    /// The raw <see cref="BasicGetResult"/> contained in this <see cref="Message"/> object.
    /// </summary>
    public BasicGetResult? RawBasicGetResult => _rawBasicGetResult;

    /// <summary>
    /// The delivery tag of this message.
    /// </summary>
    public ulong DeliveryTag => _rawBasicGetResult?.DeliveryTag ?? 0;

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
                    deliveryTag: _rawBasicGetResult.DeliveryTag,
                    redelivered: _rawBasicGetResult.Redelivered,
                    exchange: _rawBasicGetResult.Exchange,
                    routingKey: value,
                    messageCount: _rawBasicGetResult.MessageCount,
                    basicProperties: _rawBasicGetResult.BasicProperties,
                    body: _rawBasicGetResult.Body
                );
            }
        }
    }

    /// <summary>
    /// The basic properties of the message.
    /// </summary>
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
                    deliveryTag: _rawBasicGetResult.DeliveryTag,
                    redelivered: _rawBasicGetResult.Redelivered,
                    exchange: _rawBasicGetResult.Exchange,
                    routingKey: _rawBasicGetResult.RoutingKey,
                    messageCount: _rawBasicGetResult.MessageCount,
                    basicProperties: value,
                    body: _rawBasicGetResult.Body
                );
            }
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
                _rawBasicGetResult = new BasicGetResult(
                    deliveryTag: 0,
                    redelivered: false,
                    exchange: string.Empty,
                    routingKey: string.Empty,
                    messageCount: 0,
                    basicProperties: null,
                    body: value ?? null
                );
            }
            else
            {
                _rawBasicGetResult = new BasicGetResult(
                    deliveryTag: _rawBasicGetResult.DeliveryTag,
                    redelivered: _rawBasicGetResult.Redelivered,
                    exchange: _rawBasicGetResult.Exchange,
                    routingKey: _rawBasicGetResult.RoutingKey,
                    messageCount: _rawBasicGetResult.MessageCount,
                    basicProperties: _rawBasicGetResult.BasicProperties,
                    body: value ?? null
                );
            }
        }
    }

    #endregion

    #region ctos's

    /// <summary>
    /// The default constructor of the <see cref="Message"/> class.
    /// </summary>
    public Message()
    {
    }

    /// <summary>
    /// The additional parameterised constructor of the <see cref="Message"/> class.
    /// </summary>
    /// <param name="rawMessage"></param>
    public Message(BasicGetResult? rawMessage)
    {
        _rawBasicGetResult = rawMessage;
    }

    #endregion
}

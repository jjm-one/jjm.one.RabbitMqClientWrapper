using RabbitMQ.Client;

namespace jjm.one.RabbitMqClientWrapper.types
{
    public class Message
    {
        #region private members

        private BasicGetResult? rawBasicGetResult;

        #endregion

        #region public members

        public ulong DeliveryTag
        {
            get
            {
                return rawBasicGetResult is not null ? rawBasicGetResult.DeliveryTag : 0;
            }
        }

        public string RoutingKey
        {
            get
            {
                return rawBasicGetResult is not null ? rawBasicGetResult.RoutingKey : string.Empty;
            }
            set
            {
                if (rawBasicGetResult is null)
                {
                    rawBasicGetResult = new BasicGetResult(
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
                    rawBasicGetResult = new BasicGetResult(
                        deliveryTag: rawBasicGetResult.DeliveryTag,
                        redelivered: rawBasicGetResult.Redelivered,
                        exchange: rawBasicGetResult.Exchange,
                        routingKey: value,
                        messageCount: rawBasicGetResult.MessageCount,
                        basicProperties: rawBasicGetResult.BasicProperties,
                        body: rawBasicGetResult.Body
                    );
                }
            }
        }

        public IBasicProperties? BasicProperties
        {
            get
            {
                return rawBasicGetResult is not null ? rawBasicGetResult.BasicProperties : null;
            }
            set
            {
                if (rawBasicGetResult is null)
                {
                    rawBasicGetResult = new BasicGetResult(
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
                    rawBasicGetResult = new BasicGetResult(
                        deliveryTag: rawBasicGetResult.DeliveryTag,
                        redelivered: rawBasicGetResult.Redelivered,
                        exchange: rawBasicGetResult.Exchange,
                        routingKey: rawBasicGetResult.RoutingKey,
                        messageCount: rawBasicGetResult.MessageCount,
                        basicProperties: value,
                        body: rawBasicGetResult.Body
                    );
                }
            }
        }

        public ReadOnlyMemory<byte>? Body
        {
            get
            {
                return rawBasicGetResult is not null ? rawBasicGetResult.Body : null;
            }
            set
            {
                if (rawBasicGetResult is null)
                {
                    rawBasicGetResult = new BasicGetResult(
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
                    rawBasicGetResult = new BasicGetResult(
                        deliveryTag: rawBasicGetResult.DeliveryTag,
                        redelivered: rawBasicGetResult.Redelivered,
                        exchange: rawBasicGetResult.Exchange,
                        routingKey: rawBasicGetResult.RoutingKey,
                        messageCount: rawBasicGetResult.MessageCount,
                        basicProperties: rawBasicGetResult.BasicProperties,
                        body: value ?? null
                    );
                }
            }
        }

        #endregion

        #region ctos's

        public Message()
        {

        }

        public Message(BasicGetResult? rawMessage)
        {
            rawBasicGetResult = rawMessage;
        }

        #endregion
    }
}

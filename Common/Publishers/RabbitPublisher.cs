using RabbitMQ.Client;

namespace Vis.Common
{
    public partial class Publishers
    {
        static class RabbitPublisher
        {
            public static void send(IModel channel, string exchange, string routingKey,
                ReadOnlyMemory<byte> body = default)
            {
                if (channel.IsClosed)
                {
                    throw new PublisherFailException(PublisherFailReason.ChannelClosed);
                }

                channel.BasicPublish(exchange: exchange, routingKey: routingKey, body: body);
            }
        }
    }
}
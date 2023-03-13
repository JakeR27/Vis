using RabbitMQ.Client;
using Vis.Common;
using Vis.Common.Models;
using Vis.Common.Models.Messages;

namespace Vis.Common
{
    partial class Publishers
    {
        private static List<IModel> _channels = new ();
        public static class SafePublisher
        {
            private static IPublisher? _diskPublisher;
            
            public static void useChannel(IModel model)
            {
                _channels.Add(model);
            }

            public static void useDiskPublisher(IPublisher publisher)
            {
                _diskPublisher = publisher;
            }

            public static void send(string exchange, string routingKey,
                ReadOnlyMemory<byte> body = default)
            {
                var successful = false;
                for (var i = 0; i < _channels.Count && successful == false; i++)
                {
                    try
                    {
                        RabbitPublisher.send(_channels[i], exchange, routingKey, body);
                        successful = true;
                    }
                    catch (Exception ex)
                    {
                        Logs.Log(Logs.LogLevel.Warning, $"Failed to send message on channel {i}");
                        Logs.LogDebug(ex.ToString());
                    }
                }

                if (!successful)
                {
                    _diskPublisher?.send(exchange, routingKey, body);
                    Logs.Log(Logs.LogLevel.Warning, $"Message to {exchange} with key {routingKey} has been saved to disk");
                }
                Logs.Log(Logs.LogLevel.Info, $"Successfully sent message to {exchange} with key {routingKey}");
            }

            public static void sendMessage<T>(T message) where T : BaseMessage
            {
                send(exchange: message.DestinationExchange, routingKey: message.RoutingKey, body: Serializer.Serialize(message));
            }
        }
    }
    
}

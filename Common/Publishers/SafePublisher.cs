using RabbitMQ.Client;
using Vis.Common;

namespace Vis.Common
{
    internal partial class Publishers
    {
        private static List<IModel> _channels;
        static class SafePublisher
        {
            public static void useChannel(IModel model)
            {
                _channels.Add(model);
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
                    }
                }

                if (!successful)
                {
                    DiskPublisher.send(exchange, routingKey, body);
                }
            }
        }
    }
    
}

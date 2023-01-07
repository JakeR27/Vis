using RabbitMQ.Client;

namespace Vis.Common
{
    partial class Publishers
    {
        static class DiskPublisher
        {
            public static void send(string exchange, string routingKey,
                ReadOnlyMemory<byte> body = default)
            {
                // serialise to something persistent
                var message = new PublisherMessage(exchange, routingKey, body.ToArray());

                // save to disk
                using (BinaryWriter bw = new BinaryWriter(File.OpenWrite("message.bin")))
                {
                    bw.Write(message.Serialize());
                }
            }
        }
    }
}

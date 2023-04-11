using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using static Vis.Common.Logs;
namespace Vis.Common.Consumers
{
    public abstract class BaseMessageConsumer
    {
        protected IModel _channel;

        public void Attach(IModel channel, string queue)
        {
            _channel = channel;
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += _callback;
            _channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);
        }

        private void _callback(object? model, BasicDeliverEventArgs args)
        {
            _log(args);
            callback(model, args);
        }

        protected abstract void callback(object? model, BasicDeliverEventArgs args);

        private static void _log(BasicDeliverEventArgs args)
        {
            var message = $"Received on {args.RoutingKey} : {Encoding.UTF8.GetString(args.Body.ToArray())}";

            Log(LogLevel.Debug, message);
        }

    }
}
using RabbitMQ.Client.Events;
using static Vis.Common.Logs;

namespace Vis.Server.Consumers
{
    internal class OutConsumer : Common.BaseMessageConsumer
    {
        protected override void callback(object? model, BasicDeliverEventArgs args)
        {
            Log(LogLevel.Info, Constants.BODY_AS_TEXT(args.Body));
        }
    }
}
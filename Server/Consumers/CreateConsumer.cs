using RabbitMQ.Client.Events;
using Vis.Common;

namespace Vis.Server.Consumers
{
    internal class CreateConsumer : Vis.Common.BaseMessageConsumer
    {
        protected override void callback(object? model, BasicDeliverEventArgs args)
        {
            Logs.Log(Logs.LogLevel.Info, Constants.BODY_AS_TEXT(args.Body));
        }
    }
}

using RabbitMQ.Client.Events;
using Vis.Common;
using Vis.Common.Models.Messages;

namespace Vis.Server.Consumers
{
    internal class InConsumer : Vis.Common.BaseMessageConsumer
    {
        protected override void callback(object? model, BasicDeliverEventArgs args)
        {
            InVisitorMessage request = Common.Models.Serializer.Deserialize<InVisitorMessage>(args.Body.ToArray());

            ServerData.visitorsStatus[request.VisitorId] = true;

            string msg = $"IN for visitor: {request.VisitorId}";
            Logs.Log(Logs.LogLevel.Info, msg);
        }
    }
}
using RabbitMQ.Client.Events;
using Vis.Common;
using Vis.Common.Models.Messages;

namespace Vis.Server.Consumers
{
    internal class OutConsumer : Vis.Common.BaseMessageConsumer
    {
        protected override void callback(object? model, BasicDeliverEventArgs args)
        {
            OutVisitorMessage request = Common.Models.Serializer.Deserialize<OutVisitorMessage>(args.Body.ToArray());

            ServerData.visitorsStatus[request.VisitorId] = false;

            string msg = $"OUT for visitor: {request.VisitorId}";
            Logs.Log(Logs.LogLevel.Info, msg);
        }
    }
}
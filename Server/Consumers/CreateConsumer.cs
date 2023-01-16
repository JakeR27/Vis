using RabbitMQ.Client.Events;
using Vis.Common;
using Vis.Common.Models.Messages;

namespace Vis.Server.Consumers
{
    internal class CreateConsumer : Vis.Common.BaseMessageConsumer
    {
        protected override void callback(object? model, BasicDeliverEventArgs args)
        {
            CreateVisitorMessage request = Common.Models.Serializer.Deserialize<CreateVisitorMessage>(args.Body.ToArray());

            ServerData.visitors.Add(request.Visitor.Id, request.Visitor);

            string msg = $"CREATE for visitor: {request.Visitor.Name}({request.Visitor.Id})";
            Logs.Log(Logs.LogLevel.Info, msg);
        }
    }
}

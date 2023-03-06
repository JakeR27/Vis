using RabbitMQ.Client.Events;
using Vis.Common;
using Vis.Common.Consumers;
using Vis.Common.Models.Messages;
using Vis.Server.Database;

namespace Vis.Server.Consumers
{
    internal class CreateConsumer : BaseMessageConsumer
    {
        protected override void callback(object? model, BasicDeliverEventArgs args)
        {
            CreateVisitorMessage request = Common.Models.Serializer.Deserialize<CreateVisitorMessage>(args.Body.ToArray());

            Dbo.Instance.Database.GetCollection<Common.Models.Visitor>("people").InsertOne(request.Visitor);
            
            ServerData.visitors.Add(request.Visitor.Guid, request.Visitor);

            string msg = $"CREATE for visitor: {request.Visitor.Name}({request.Visitor.Guid})";
            Logs.Log(Logs.LogLevel.Info, msg);
        }
    }
}

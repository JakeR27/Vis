using RabbitMQ.Client.Events;
using Vis.Common;
using Vis.Common.Models.Messages;
using Vis.Server.Database;
using Vis.Server.Models;

namespace Vis.Server.Consumers
{
    internal class InConsumer : Vis.Common.BaseMessageConsumer
    {
        protected override void callback(object? model, BasicDeliverEventArgs args)
        {
            var request = Common.Models.Serializer.Deserialize<InVisitorMessage>(args.Body.ToArray());

            var visitorEvent = new VisitorEvent()
            {
                VisitorId = request.VisitorId,
                EventType = VisitorEventEnum.In,
                Timestamp = request.Time
            };

            using (var session = Dbo.Instance.Client.StartSession())
            {
                Logs.LogDebug("Attempting to start MongoDB transaction");
                session.StartTransaction();
                Dbo.Instance.GetCollection<VisitorEvent>("events").InsertOne(visitorEvent);
                session.CommitTransaction();
                Logs.LogDebug("MongoDB transaction committed successfully");
            }
           
            

            ServerData.visitorsStatus[request.VisitorId] = true;

            string msg = $"IN for visitor: {request.VisitorId}";
            Logs.Log(Logs.LogLevel.Info, msg);
        }
    }
}
using MongoDB.Driver;
using RabbitMQ.Client.Events;
using Vis.Common;
using Vis.Common.Consumers;
using Vis.Common.Models;
using Vis.Common.Models.Messages;
using Vis.Server.Database;
using Vis.Server.Models;

namespace Vis.Server.Consumers
{
    internal class InConsumer : BaseMessageConsumer
    {
        protected override void callback(object? model, BasicDeliverEventArgs args)
        {
            // deserialize incoming message
            var request = Common.Models.Serializer.Deserialize<InVisitorMessage>(args.Body.ToArray());

            // convert to visitor event
            var incomingVisitorEvent = new VisitorEvent()
            {
                VisitorId = request.VisitorId,
                EventType = VisitorEventEnum.In,
                Timestamp = request.Time
            };
            //
            // // get the current most recent event 
            // var currentVisitorEvent = Dbo.Instance.GetCollection<VisitorEvent>("events")
            //         .Find(visitorEvent => visitorEvent.VisitorId == incomingVisitorEvent.VisitorId)
            //         .Sort("{Timestamp:-1}")
            //         .Limit(1).ToList()[0];
            //
            // // if needs updating
            // if (currentVisitorEvent.Timestamp < incomingVisitorEvent.Timestamp)
            // {
            //     var filter = Builders<VisitorEvent>.Filter
            //         .Eq(visitorEvent => visitorEvent.Id, currentVisitorEvent.Id);
            //     
            //     var update = Builders<VisitorEvent>.Update
            //         .Set(visitorEvent => visitorEvent.EventType, incomingVisitorEvent.EventType);
            //     
            //     Dbo.Instance.GetCollection<VisitorEvent>("events").UpdateOne(filter, update);
            // }

            Dbo.Instance.InsertOne("events", incomingVisitorEvent);
           
            

            ServerData.visitorsStatus[request.VisitorId] = VisitorEventEnum.In;

            string msg = $"IN for visitor: {request.VisitorId}";
            Logs.Log(Logs.LogLevel.Info, msg);
        }
    }
}
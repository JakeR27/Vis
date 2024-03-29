﻿using RabbitMQ.Client.Events;
using Vis.Common;
using Vis.Common.Consumers;
using Vis.Common.Models;
using Vis.Common.Models.Messages;
using Vis.Server.Database;
using Vis.Server.Models;

namespace Vis.Server.Consumers
{
    internal class OutConsumer : BaseMessageConsumer
    {
        protected override void callback(object? model, BasicDeliverEventArgs args)
        {
            OutVisitorMessage request = Common.Models.Serializer.Deserialize<OutVisitorMessage>(args.Body.ToArray());

            var visitorEvent = new VisitorEvent()
            {
                VisitorId = request.VisitorId,
                EventType = VisitorEventEnum.Out,
                Timestamp = request.Time
            };

            Dbo.Instance.InsertOne("events", visitorEvent);
            
            ServerData.visitorsStatus[request.VisitorId] = VisitorEventEnum.Out;

            string msg = $"OUT for visitor: {request.VisitorId}";
            Logs.Log(Logs.LogLevel.Info, msg);
        }
    }
}
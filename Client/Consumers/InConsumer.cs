using System.Runtime.CompilerServices;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Vis.Common;
using Vis.Common.Consumers;
using Vis.Common.Models;
using Vis.Common.Models.Messages;

namespace Vis.Client.Consumers
{
    internal class InConsumer : BaseMessageConsumer
    {
        protected override void callback(object? model, BasicDeliverEventArgs args)
        {
            InVisitorMessage message = Common.Models.Serializer.Deserialize<InVisitorMessage>(args.Body.ToArray());
            string msg = $"Received IN message: {message.VisitorId}";
            Logs.Log(Logs.LogLevel.Info, msg);

            ClientState.visitorsStatus[message.VisitorId] = VisitorEventEnum.In;
            ClientState.displayVisitors();
        }
    }
}
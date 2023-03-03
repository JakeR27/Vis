using System.Runtime.CompilerServices;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Vis.Common;
using Vis.Common.Models.Messages;

namespace Vis.Client.Consumers
{
    internal class OutConsumer : BaseMessageConsumer
    {
        protected override void callback(object? model, BasicDeliverEventArgs args)
        {
            OutVisitorMessage message = Common.Models.Serializer.Deserialize<OutVisitorMessage>(args.Body.ToArray());
            string msg = $"Received OUT message: {message.VisitorId}";
            Logs.Log(Logs.LogLevel.Info, msg);

            ClientState.visitorsStatus[message.VisitorId] = false;

            ClientState.displayVisitors();
        }
    }
}
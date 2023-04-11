using System.Runtime.CompilerServices;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Vis.Common;
using Vis.Common.Consumers;
using Vis.Common.Models.Messages;

namespace Vis.Client.Consumers
{
    internal class CreateConsumer : BaseMessageConsumer
    {
        protected override void callback(object? model, BasicDeliverEventArgs args)
        {
            CreateVisitorMessage message = Common.Models.Serializer.Deserialize<CreateVisitorMessage>(args.Body.ToArray());
            string msg = $"Received CREATE message: {message.Visitor.Name}, id({message.Visitor.Guid})";
            Logs.Log(Logs.LogLevel.Info, msg);

            ClientState.visitors.Add(message.Visitor.Guid, message.Visitor);
            ClientState.displayVisitors();
        }
    }
}
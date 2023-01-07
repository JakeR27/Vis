using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Vis.Common;

namespace Vis.Client.Consumers
{
    internal class AuthConsumer : BaseMessageConsumer
    {
        protected override void callback(object? model, BasicDeliverEventArgs args)
        {
            string msg = $"Received AUTH message {Constants.BODY_AS_TEXT(args.Body)}";
            Logs.Log(Logs.LogLevel.Info, msg);

            ClientData._organisationExchangeName = Constants.BODY_AS_TEXT(args.Body);
            ClientData._organisationExchangeFound = true;
        }
    }
}
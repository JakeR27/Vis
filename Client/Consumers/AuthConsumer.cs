using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Vis.Common;
using Vis.Common.Models.Messages;

namespace Vis.Client.Consumers
{
    internal class AuthConsumer : BaseMessageConsumer
    {
        protected override void callback(object? model, BasicDeliverEventArgs args)
        {
            AuthResponseMessage response = Common.Models.Serializer.Deserialize<AuthResponseMessage>(args.Body.ToArray());
            string msg = $"Received AUTH response, success: {response.Success}, organisation xch: {response.OrganisationExchangeName}";
            Logs.Log(Logs.LogLevel.Info, msg);

            ClientData._organisationExchangeName = response.OrganisationExchangeName;
            ClientData._organisationExchangeFound = response.Success;
        }
    }
}
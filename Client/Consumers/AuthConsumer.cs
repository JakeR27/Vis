using RabbitMQ.Client.Events;
using Vis.Client.Startup;
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

            if (response.Success && AttachBindings.AuthState == State.WAITING)
            {
                AttachBindings.AuthState = State.COMPLETE;
            }

            ClientState._organisationExchangeName = response.OrganisationExchangeName;
            ClientState._organisationExchangeFound = response.Success;
        }
    }
}
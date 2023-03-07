using RabbitMQ.Client.Events;
using Vis.Client.Startup;
using Vis.Common;
using Vis.Common.Consumers;
using Vis.Common.Models.Messages;
using Vis.Common.Startup;

namespace Vis.Client.Consumers
{
    internal class AuthConsumer : BaseMessageConsumer
    {
        protected override void callback(object? model, BasicDeliverEventArgs args)
        {
            AuthResponseMessage response = Common.Models.Serializer.Deserialize<AuthResponseMessage>(args.Body.ToArray());
            string msg = $"Received AUTH response, success: {response.Success}, organisation xch: {response.OrganisationExchangeName}";
            Logs.Log(Logs.LogLevel.Info, msg);

            if (AttachBindings.AuthState == State.WAITING)
            {
                if (response.Success == true)
                {
                    AttachBindings.AuthState = State.COMPLETE;
                    ClientState._organisationExchangeName = response.OrganisationExchangeName;
                    ClientState._organisationExchangeFound = response.Success;
                }
                else
                {
                    Logs.LogError("Auth with server failed. Check SECRET is correct");
                }
            }
            else
            {
                Logs.LogWarning("Got auth response, but no auth request was sent");
            }
        }
    }
}
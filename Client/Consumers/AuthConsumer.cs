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
        private BaseStartupTask _authStartupTask;

        protected override void callback(object? model, BasicDeliverEventArgs args)
        {
            AuthResponseMessage response = Common.Models.Serializer.Deserialize<AuthResponseMessage>(args.Body.ToArray());
            string msg = $"Received AUTH response, success: {response.Success}, organisation xch: {response.OrganisationExchangeName}";
            Logs.Log(Logs.LogLevel.Info, msg);

            Logs.LogDebug($"AuthState is {_authStartupTask.TaskState}");
            
            if (_authStartupTask.TaskState == State.WAITING)
            {
                Logs.LogDebug("Inside auth status == waiting");
                if (response.Success == true)
                {
                    Logs.LogDebug("inside response.success == true");
                    
                    _authStartupTask.ExternalComplete();
                    
                    Logs.LogDebug("after set authstate = COMPLETE");
                    ClientState._organisationExchangeName = response.OrganisationExchangeName;
                }
                else
                {
                    Logs.LogError("Auth with server failed. Check SECRET is correct");
                    _authStartupTask.ExternalFail("Server authentication failed");
                }
            }
            else
            {
                Logs.LogWarning("Got auth response, but no auth request was sent");
            }
        }
        
        public void CompletesStartupTask(BaseStartupTask task)
        {
            _authStartupTask = task;
        }
    }
}
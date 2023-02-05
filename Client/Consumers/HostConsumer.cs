using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Vis.Common;
using Vis.Common.Models.Messages;
using Vis.Common.Models.Results;

namespace Vis.Client.Consumers
{
    internal class HostConsumer : BaseMessageConsumer
    {
        protected override void callback(object? model, BasicDeliverEventArgs args)
        {
            HostResponseMessage response = Common.Models.Serializer.Deserialize<HostResponseMessage>(args.Body.ToArray());
            string msg = $"Received HOST response, host: {response.Host}, port: {response.Port}";
            Logs.Log(Logs.LogLevel.Info, msg);

            var httpClient = new HttpClient();
            var visitorData = httpClient.GetStringAsync($"http://{response.Host}:{response.Port}/visitors").Result;

            List<VisitorResult> parsedData =
                Vis.Common.Models.Serializer.DeserializeJson <List<VisitorResult>>(visitorData);

            foreach (var visitorResult in parsedData)
            {
                ClientData.visitors[visitorResult.Visitor.Guid] = visitorResult.Visitor;
                ClientData.visitorsStatus[visitorResult.Visitor.Guid] = visitorResult.status;
            }

            ClientData._serverHostFound = true;
        }
    }
}
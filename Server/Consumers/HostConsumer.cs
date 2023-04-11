using RabbitMQ.Client.Events;
using Vis.Common;
using Vis.Common.Consumers;
using Vis.Common.Models;
using Vis.Common.Models.Messages;
using static Vis.Common.Logs;

namespace Vis.Server.Consumers
{
    internal class HostConsumer : BaseMessageConsumer
    {
        protected override void callback(object? model, BasicDeliverEventArgs args)
        {
            // parse request
            HostRequestMessage request = Serializer.Deserialize<HostRequestMessage>(args.Body.ToArray());

            // get current global ip from provider
            var httpClient = new HttpClient();
            var ip = httpClient.GetStringAsync("https://api.ipify.org").Result;

            // create response with data
            var response = new HostResponseMessage(request.OrganisationId, request.UnitId)
            {
                Host = ip,
                Port = 5000
            };

            // send response
            Publishers.SafePublisher.send(response);

            // log it
            Log(LogLevel.Info, Constants.BODY_AS_TEXT(args.Body));
        }
    }
}
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using MongoDB.Driver;
using Vis.Common;
using Vis.Common.Consumers;
using Vis.Common.Models.Messages;
using Vis.Server.Database;
using Vis.Server.Models;

namespace Vis.Server.Consumers
{
    internal class AuthConsumer : BaseMessageConsumer
    {
        protected override void callback(object? model, BasicDeliverEventArgs args)
        {
            AuthRequestMessage request = Common.Models.Serializer.Deserialize<AuthRequestMessage>(args.Body.ToArray());
            // parse routeing key
            var organisationId = request.OrganisationId;
            var unitId = request.UnitId;
            var ORG_XCH = Vis.Constants.ORGANISATION_XCH(organisationId);

            // LOG
            var logMsg = $"AUTH request received for {request.OrganisationId}.{request.UnitId} with {request.Secret}";
            Logs.Log(Logs.LogLevel.Info, logMsg);


            // VALIDATE SECRET
            var requiredSecret = Dbo.Instance
                .GetCollection<OrganisationSecret>("secrets")
                .Find(secret => secret.OrganisationId == organisationId)
                .First();

            if (request.Secret == string.Empty || request.Secret != requiredSecret.Value)
            {
                Logs.LogInfo($"Auth check failed for {organisationId}.{unitId}");
                Logs.LogDebug($"{request.Secret} did not match {requiredSecret.Value}");
                Publishers.SafePublisher.sendMessage(new AuthResponseMessage(organisationId, unitId)
                {
                    Success = false,
                    OrganisationExchangeName = ORG_XCH
                });
                return;
            }


            // create and link exchange
            _channel.ExchangeDeclare(
                exchange: ORG_XCH, 
                type: ExchangeType.Topic
            );
            _channel.ExchangeBind(
                source: ORG_XCH, 
                destination: Constants.BACKEND_XCH, 
                routingKey: "*.*"
            );

            // reply with organisation exchange name

            var response = new Common.Models.Messages.AuthResponseMessage(organisationId, unitId)
            {
                Success = true,
                OrganisationExchangeName = ORG_XCH
            };

            Publishers.SafePublisher.sendMessage(response);

            //var message = Encoding.UTF8.GetBytes(ORG_XCH);
            //Publishers.SafePublisher.send(
            //    exchange: Constants.DISCOVERY_XCH, 
            //    routingKey: Constants.AUTH_RESPONSE_KEY(organisationId, unitId), 
            //    body: message
            //);
        }
    }
}

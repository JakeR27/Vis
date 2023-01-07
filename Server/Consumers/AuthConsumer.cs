using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Vis.Common;

namespace Vis.Server.Consumers
{
    internal class AuthConsumer : Common.BaseMessageConsumer
    {
        protected override void callback(object? model, BasicDeliverEventArgs args)
        {
            // parse routeing key
            var routingKeyParts = args.RoutingKey.Split(".");
            var organisationId = int.Parse(routingKeyParts[0]);
            var unitId = int.Parse(routingKeyParts[1]);


            // create and link exchange
            var ORG_XCH = Vis.Constants.ORGANISATION_XCH(organisationId);
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
            var message = Encoding.UTF8.GetBytes(ORG_XCH);
            Publishers.SafePublisher.send(
                exchange: Constants.DISCOVERY_XCH, 
                routingKey: Constants.AUTH_RESPONSE_KEY(organisationId, unitId), 
                body: message
            );
        }
    }
}

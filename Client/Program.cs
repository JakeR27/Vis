using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Vis.Client.Consumers;
using Vis.Common;
using Vis.Common.Models;
using Vis.Common.Models.Messages;

namespace Vis.Client
{
    class Program
    {
        private static IModel _channel;

        public static void Main()
        {
            var factory = new ConnectionFactory()
                { HostName = "ec2-13-42-23-89.eu-west-2.compute.amazonaws.com", UserName = "backend", Password = "backend" };


            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            Vis.Common.Publishers.SafePublisher.useChannel(_channel);

        
            var createQueue = _channel.QueueDeclare().QueueName;
            var inQueue = _channel.QueueDeclare().QueueName;
            var outQueue = _channel.QueueDeclare().QueueName;
            var hostQueue = _channel.QueueDeclare().QueueName;
            var authQueue = _channel.QueueDeclare().QueueName;

            string hostRequestsRoutingKey = $"{ClientData._organisationId}.{ClientData._unitId}.requests.host";
            string authRequestsRoutingKey = $"{ClientData._organisationId}.{ClientData._unitId}.requests.auth";
            string hostResponsesRoutingKey = $"{ClientData._organisationId}.{ClientData._unitId}.responses.host";
            string authResponsesRoutingKey = $"{ClientData._organisationId}.{ClientData._unitId}.responses.auth";


            _channel.ExchangeDeclare(exchange: "discovery-xch", type: ExchangeType.Topic);
            _channel.QueueBind(queue: hostQueue, exchange: "discovery-xch", routingKey: hostResponsesRoutingKey);
            _channel.QueueBind(queue: authQueue, exchange: "discovery-xch", routingKey: authResponsesRoutingKey);
            
            new AuthConsumer().Attach(_channel, authQueue);

            var authRequest =
                new Common.Models.Messages.AuthRequestMessage(ClientData._organisationId, ClientData._unitId, "SECRET");

            Publishers.SafePublisher.sendMessage(authRequest);
            //Publishers.SafePublisher.send(exchange: Constants.DISCOVERY_XCH, routingKey: authRequestsRoutingKey, body: Encoding.UTF8.GetBytes("CREATE MESSAGE"));

            while (!ClientData._organisationExchangeFound) { } //wait

            _channel.ExchangeDeclare(exchange: ClientData._organisationExchangeName, type: ExchangeType.Topic);
            _channel.QueueBind(queue: createQueue, exchange: ClientData._organisationExchangeName, routingKey: "*.create");
            _channel.QueueBind(queue: inQueue,     exchange: ClientData._organisationExchangeName, routingKey: "*.in");
            _channel.QueueBind(queue: outQueue,    exchange: ClientData._organisationExchangeName, routingKey: "*.out");

            var temp = new CreateVisitorMessage
            {
                Visitor =
                {
                    Id = Guid.NewGuid(),
                    Name = "bob"
                },
                DestinationExchange = ClientData._organisationExchangeName,
                RoutingKey = "10.create"
            };

            Console.WriteLine(Encoding.UTF8.GetString(Vis.Common.Models.Serializer.Serialize(temp)));

            Publishers.SafePublisher.sendMessage(temp);
            //Publishers.SafePublisher.send(exchange: ClientData._organisationExchangeName, routingKey: $"{ClientData._organisationId}.create", body: Serializer.Serialize(temp));
            //Publishers.SafePublisher.send(exchange: ClientData._organisationExchangeName, routingKey: $"{ClientData._organisationId}.create", body: Encoding.UTF8.GetBytes("CREATE message content"));
            Publishers.SafePublisher.send(exchange: ClientData._organisationExchangeName, routingKey: $"{ClientData._organisationId}.in",     body: Encoding.UTF8.GetBytes("IN message content"));
            Publishers.SafePublisher.send(exchange: ClientData._organisationExchangeName, routingKey: $"{ClientData._organisationId}.out",    body: Encoding.UTF8.GetBytes("OUT message content"));

        }
    }
}
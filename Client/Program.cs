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

            Console.WriteLine("Enter unit id");
            ClientData._unitId = int.Parse(Console.ReadLine());

        
            var createQueue = _channel.QueueDeclare().QueueName;
            var inQueue = _channel.QueueDeclare().QueueName;
            var outQueue = _channel.QueueDeclare().QueueName;
            var hostQueue = _channel.QueueDeclare().QueueName;
            var authQueue = _channel.QueueDeclare().QueueName;
            
            _channel.ExchangeDeclare(exchange: Constants.DISCOVERY_XCH, type: ExchangeType.Topic);
            _channel.QueueBind(queue: hostQueue, exchange: Constants.DISCOVERY_XCH, routingKey: Constants.HOST_RESPONSE_KEY(ClientData._organisationId, ClientData._unitId));
            _channel.QueueBind(queue: authQueue, exchange: Constants.DISCOVERY_XCH, routingKey: Constants.AUTH_RESPONSE_KEY(ClientData._organisationId, ClientData._unitId));
            
            new AuthConsumer().Attach(_channel, authQueue);
            new HostConsumer().Attach(_channel, hostQueue);
            new CreateConsumer().Attach(_channel, createQueue);
            new InConsumer().Attach(_channel, inQueue);
            new OutConsumer().Attach(_channel, outQueue);

            

            var authRequest = new AuthRequestMessage(ClientData._organisationId, ClientData._unitId, "SECRET");
            var hostRequest = new HostRequestMessage(ClientData._organisationId, ClientData._unitId);

            Publishers.SafePublisher.sendMessage(authRequest);
            Publishers.SafePublisher.sendMessage(hostRequest);

            while (!ClientData._organisationExchangeFound) { } //wait

            _channel.ExchangeDeclare(exchange: ClientData._organisationExchangeName, type: ExchangeType.Topic);
            _channel.QueueBind(queue: createQueue, exchange: ClientData._organisationExchangeName, routingKey: $"{ClientData._organisationId}.create");
            _channel.QueueBind(queue: inQueue,     exchange: ClientData._organisationExchangeName, routingKey: $"{ClientData._organisationId}.in");
            _channel.QueueBind(queue: outQueue,    exchange: ClientData._organisationExchangeName, routingKey: $"{ClientData._organisationId}.out");

           
            while (!ClientData._serverHostFound) { } // wait
            ClientData.displayVisitors();

            var running = true;
            while (running)
            {
                Console.WriteLine("enter a name");
                var input = Console.ReadLine();

                if (input is "q" or "quit") {running = false; continue;}

                switch (input[0])
                {
                    case 'c':
                        Publishers.SafePublisher.sendMessage(new CreateVisitorMessage()
                        {
                            Visitor =
                            {
                                Id = Guid.NewGuid(),
                                Name = input[2..]
                            },
                            DestinationExchange = ClientData._organisationExchangeName,
                            RoutingKey = "10.create"
                        });
                        break;
                    case 'i':
                        Publishers.SafePublisher.sendMessage(new InVisitorMessage()
                        {
                            VisitorId = ClientData.visitors.Values.ToList()[int.Parse(input[2].ToString())].Id,
                            Time = DateTime.Now,
                            DestinationExchange = ClientData._organisationExchangeName,
                            RoutingKey = "10.in"
                        });
                        break;
                    case 'o':
                        Publishers.SafePublisher.sendMessage(new OutVisitorMessage()
                        {
                            VisitorId = ClientData.visitors.Values.ToList()[int.Parse(input[2].ToString())].Id,
                            Time = DateTime.Now,
                            DestinationExchange = ClientData._organisationExchangeName,
                            RoutingKey = "10.out"
                        });
                        break;
                }

                
            }

        }
    }
}
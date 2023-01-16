using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Vis.Server.Consumers;
using Vis.Server.Endpoints;

namespace Vis.Server
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

            _channel.QueueDeclare(
                queue: "backend-create",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            _channel.QueueDeclare(
                queue: "backend-in",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            _channel.QueueDeclare(
                queue: "backend-out",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            _channel.QueueDeclare(
                queue: "backend-requests-host",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            _channel.QueueDeclare(
                queue: "backend-requests-auth",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _channel.ExchangeDeclare(exchange: Constants.BACKEND_XCH, type: ExchangeType.Topic);
            _channel.QueueBind(
                queue: "backend-create", 
                exchange: Constants.BACKEND_XCH, 
                routingKey: "*.create"
            );
            _channel.QueueBind(
                queue: "backend-in", 
                exchange: Constants.BACKEND_XCH, 
                routingKey: "*.in"
            );
            _channel.QueueBind(
                queue: "backend-out", 
                exchange: Constants.BACKEND_XCH, 
                routingKey: "*.out"
            );

            _channel.ExchangeDeclare(exchange: "discovery-xch", type: ExchangeType.Topic);
            _channel.QueueBind(
                queue: "backend-requests-host", 
                exchange: Constants.DISCOVERY_XCH,
                routingKey: "*.*.requests.host"
            );
            _channel.QueueBind(
                queue: "backend-requests-auth", 
                exchange: Constants.DISCOVERY_XCH,
                routingKey: "*.*.requests.auth"
            );

            new CreateConsumer().Attach(_channel, "backend-create");
            new InConsumer().Attach(_channel, "backend-in");
            new OutConsumer().Attach(_channel, "backend-out");
            new HostConsumer().Attach(_channel, "backend-requests-host");
            new AuthConsumer().Attach(_channel, "backend-requests-auth");
            new GetVisitors("/visitors").handle();
            Vis.WebServer.App.WebApp.Urls.Add("http://*:5000");
            Vis.WebServer.App.WebApp.RunAsync();

            Console.WriteLine("Listening for messages, press [Enter] to exit");
            Console.ReadLine();

        }
    }
}
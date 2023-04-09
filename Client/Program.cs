using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Vis.Client.Consumers;
using Vis.Client.Database;
using Vis.Client.Messages;
using Vis.Client.Startup;
using Vis.Common;
using Vis.Common.Configuration;
using Vis.Common.Models;
using Vis.Common.Models.Messages;
using Vis.Common.RabbitMq;
using Vis.Common.Startup;

namespace Vis.Client
{
    public class Program
    {
        private static IModel _channel;

        public static void GracefulExit(int code)
        {
            if (RmqConnect.Connection is { IsOpen: true })
            {
                RmqConnect.Connection.Close();
                RmqConnect.Connection.Dispose();
            }
            Environment.Exit(code);
        }
        
        public static void Main()
        {
            // var connection = Vis.Common.RabbitMq.RmqConnect.Connect("backend", "backend");
            // _channel = connection.CreateModel();
            // Vis.Common.Publishers.SafePublisher.useChannel(_channel);
            //
            // Console.WriteLine("Enter unit id");
            // //ClientData._unitId = int.Parse(Console.ReadLine());
            // ClientData._unitId = 1;
            //
            //
            // var createQueue = _channel.QueueDeclare().QueueName;
            // var inQueue = _channel.QueueDeclare().QueueName;
            // var outQueue = _channel.QueueDeclare().QueueName;
            // var hostQueue = _channel.QueueDeclare().QueueName;
            // var authQueue = _channel.QueueDeclare().QueueName;
            //
            // _channel.ExchangeDeclare(exchange: Constants.DISCOVERY_XCH, type: ExchangeType.Topic);
            // _channel.QueueBind(queue: hostQueue, exchange: Constants.DISCOVERY_XCH, routingKey: Constants.HOST_RESPONSE_KEY(ClientData._organisationId, ClientData._unitId));
            // _channel.QueueBind(queue: authQueue, exchange: Constants.DISCOVERY_XCH, routingKey: Constants.AUTH_RESPONSE_KEY(ClientData._organisationId, ClientData._unitId));
            //
            // new AuthConsumer().Attach(_channel, authQueue);
            // new HostConsumer().Attach(_channel, hostQueue);
            // new CreateConsumer().Attach(_channel, createQueue);
            // new InConsumer().Attach(_channel, inQueue);
            // new OutConsumer().Attach(_channel, outQueue);
            //
            //
            //
            // var authRequest = new AuthRequestMessage(ClientData._organisationId, ClientData._unitId, "SECRET");
            // var hostRequest = new HostRequestMessage(ClientData._organisationId, ClientData._unitId);
            //
            // Publishers.SafePublisher.sendMessage(authRequest);
            // Publishers.SafePublisher.sendMessage(hostRequest);
            //
            // while (!ClientData._organisationExchangeFound) { } //wait
            //
            // _channel.ExchangeDeclare(exchange: ClientData._organisationExchangeName, type: ExchangeType.Topic);
            // _channel.QueueBind(queue: createQueue, exchange: ClientData._organisationExchangeName, routingKey: $"{ClientData._organisationId}.create");
            // _channel.QueueBind(queue: inQueue,     exchange: ClientData._organisationExchangeName, routingKey: $"{ClientData._organisationId}.in");
            // _channel.QueueBind(queue: outQueue,    exchange: ClientData._organisationExchangeName, routingKey: $"{ClientData._organisationId}.out");
            //
            //
            // while (!ClientData._serverHostFound) { } // wait
            // ClientData.displayVisitors();
            //
            Logs.Log(Logs.LogLevel.Debug, "Beginning startup");

            
            
            //Vis.Client.Startup.AttachBindings.Begin();
            // new RabbitMqConnector().Execute();
            // var channel = RmqConnect.Connection.CreateModel();
            //
            // var AuthTask = new AuthNegotiator();
            // AuthTask.Setup(channel);
            // AuthTask.Execute();
            //
            // // wait for auth task
            // while (AuthTask.TaskState != State.COMPLETE) {}
            //
            // var HostTask = new HostNegotiator();
            // HostTask.Setup(channel);
            // HostTask.Execute();
            
            // Logs.Log(Logs.LogLevel.Debug, "Waiting for startup to complete");
            // while (HostTask.TaskState != State.COMPLETE) {} //wait for completion

            var taskRunner = new TaskRunner();
            taskRunner.TaskRunnerCancelled += (sender, args) =>
            {
                Logs.LogError("Task runner cancelled. Exiting...");
                GracefulExit(1);
            };
            
            taskRunner.QueueTask(new ConfigurationParser());
            taskRunner.QueueTask(new RabbitMqConnector());
            taskRunner.QueueTask(new DiskPublisherRegistrar());
            taskRunner.QueueTask(new Resender());
            taskRunner.QueueTask(new AuthNegotiator());
            taskRunner.QueueTask(new HostNegotiator());
            
            var setupComplete = taskRunner.Run();
            
            if (setupComplete == State.ERROR)
            {
                GracefulExit(1);
            }

            // LiteDb.Instance.Insert<InVisitorMessage>(new InVisitorMessage()
            // {
            //     DestinationExchange = ClientState._organisationExchangeName,
            //     Id = Guid.NewGuid(),
            //     RoutingKey = $"{ClientState._organisationId}.in",
            //     Time = DateTime.UtcNow,
            //     VisitorId = Guid.NewGuid()
            // });
            
            
            Logs.Log(Logs.LogLevel.Info, "Startup completed. Application running...");
            
            
            var running = true;
            ClientState.displayVisitors();
            while (running)
            {
                //ClientState.displayVisitors();
                //continue;
                Console.WriteLine("enter command");
                var input = Console.ReadLine();

                if (input is "q" or "quit") {running = false; continue;}
                if (input.Length <= 2) {continue;}

                switch (input[0])
                {
                    case 'c':
                        Common.Publishers.SafePublisher.send(new CreateVisitorMessage()
                        {
                            Visitor =
                            {
                                Guid = Guid.NewGuid(),
                                OrganisationId = ClientState._organisationId,
                                Name = input[2..]
                            },
                            DestinationExchange = ClientState._organisationExchangeName,
                            RoutingKey = $"{ClientState._organisationId}.create"
                        });
                        break;
                    case 'i':
                        Common.Publishers.SafePublisher.send(new InVisitorMessage()
                        {
                            VisitorId = ClientState.visitors.Values.ToList()[int.Parse(input[2..].ToString())].Guid,
                            Time = DateTime.UtcNow,
                            DestinationExchange = ClientState._organisationExchangeName,
                            RoutingKey = $"{ClientState._organisationId}.in"
                        });
                        break;
                    case 'o':
                        Common.Publishers.SafePublisher.send(new OutVisitorMessage()
                        {
                            VisitorId = ClientState.visitors.Values.ToList()[int.Parse(input[2..].ToString())].Guid,
                            Time = DateTime.UtcNow,
                            DestinationExchange = ClientState._organisationExchangeName,
                            RoutingKey = $"{ClientState._organisationId}.out"
                        });
                        break;
                    case 'd':
                        ClientState.displayVisitors();
                        break;
                    case 'q':
                        GracefulExit(0);
                        break;
                }

                
            }

        }
    }
}
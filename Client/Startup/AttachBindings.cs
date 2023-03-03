using RabbitMQ.Client;
using Vis.Client.Consumers;
using Vis.Client.Startup;
using Vis.Common;
using Vis.Common.Models.Messages;
using Constants = Vis.Constants;

namespace Vis.Client.Startup;

public class AttachBindings
{
    public static State BindingState = State.STARTED;
    public static State AuthState = State.STARTED;
    public static State HostState = State.STARTED;

    public static bool Completed => AuthState == State.COMPLETE && HostState == State.COMPLETE;

    private static string _hostQ, _authQ, _createQ, _inQ, _outQ;

    public static void Begin()
    {
        var connection = Common.RabbitMq.RmqConnect.Connect("backend", "backend");
        var channel = connection.CreateModel();
        Publishers.SafePublisher.useChannel(channel);
        Logs.Log(Logs.LogLevel.Debug, "Successfully connected to RMQ and bound channel for sending");
        
        _bindDiscoveryQueues(channel);
        _attachDiscoveryConsumers(channel);
        _sendDiscoveryMessages();
        _bindOperationalQueues(channel);
        _attachOperationalConsumers(channel);
        _sendHostMessages();

        Logs.Log(Logs.LogLevel.Debug, "Waiting for auth and host responses");
        //while (AuthState != State.COMPLETE && HostState != State.COMPLETE) { } //wait until complete
        //Logs.Log(Logs.LogLevel.Debug, "auth and host responses received");
    }

    private static void _bindDiscoveryQueues(IModel channel)
    {
        _hostQ = channel.QueueDeclare();
        _authQ = channel.QueueDeclare();
        
        channel.ExchangeDeclare(exchange: Constants.DISCOVERY_XCH, type: ExchangeType.Topic);
        
        //TODO: make these use program data to define organisation and unit IDs
        channel.QueueBind(queue: _hostQ, exchange: Constants.DISCOVERY_XCH, routingKey: Constants.HOST_RESPONSE_KEY(10, 1));
        channel.QueueBind(queue: _authQ, exchange: Constants.DISCOVERY_XCH, routingKey: Constants.AUTH_RESPONSE_KEY(10, 1));
    }

    private static void _attachDiscoveryConsumers(IModel channel)
    {
        new AuthConsumer().Attach(channel, _authQ);
        new HostConsumer().Attach(channel, _hostQ);
    }

    private static void _sendDiscoveryMessages()
    {
        AuthState = State.WAITING;
        
        //TODO: replace with specific client data
        var authRequest = new AuthRequestMessage(10, 1, "SECRET");
        Publishers.SafePublisher.sendMessage(authRequest);
    }

    private static void _bindOperationalQueues(IModel channel)
    {
        _createQ = channel.QueueDeclare();
        _inQ = channel.QueueDeclare();
        _outQ= channel.QueueDeclare();
        
        channel.ExchangeDeclare(exchange: Constants.ORGANISATION_XCH(10), type: ExchangeType.Topic);
        
        //TODO: replace with specific client data
        
        channel.QueueBind(queue: _createQ, exchange: Constants.ORGANISATION_XCH(10), routingKey: $"{10}.create");
        channel.QueueBind(queue: _inQ,     exchange: Constants.ORGANISATION_XCH(10), routingKey: $"{10}.in");
        channel.QueueBind(queue: _outQ,    exchange: Constants.ORGANISATION_XCH(10), routingKey: $"{10}.out");
    }

    private static void _attachOperationalConsumers(IModel channel)
    {
        new CreateConsumer().Attach(channel, _createQ);
        new InConsumer().Attach(channel, _inQ);
        new OutConsumer().Attach(channel, _outQ);
    }

    private static void _sendHostMessages()
    {
        HostState = State.WAITING;
        
        //TODO: replace with specific client data
        var hostRequest = new HostRequestMessage(10, 1);
        Publishers.SafePublisher.sendMessage(hostRequest);
    }
}
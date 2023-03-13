using RabbitMQ.Client;
using Vis.Client.Consumers;
using Vis.Client.Startup;
using Vis.Common;
using Vis.Common.Models.Messages;
using Vis.Common.Startup;
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

        // _bindDiscoveryQueues(channel);
        // _attachDiscoveryConsumers(channel);
        // _sendDiscoveryMessages();
        //
        // // wait for auth to complete before trying to send further messages
        // Logs.LogInfo("Waiting for auth to complete...");
        // while (AttachBindings.AuthState != State.COMPLETE)
        // {
        //     //Logs.LogDebug($"AuthState = {AuthState}");
        // }
        // Logs.LogInfo("Auth completed");
        //
        // _bindOperationalQueues(channel);
        // _attachOperationalConsumers(channel);
        // _sendHostMessages();

        Logs.Log(Logs.LogLevel.Debug, "Waiting for host responses");
        //while (AuthState != State.COMPLETE && HostState != State.COMPLETE) { } //wait until complete
        //Logs.Log(Logs.LogLevel.Debug, "auth and host responses received");
    }

    private static void _bindDiscoveryQueues(IModel channel)
    {
        _hostQ = channel.QueueDeclare();
        _authQ = channel.QueueDeclare();
        
        channel.ExchangeDeclare(exchange: Constants.DISCOVERY_XCH, type: ExchangeType.Topic);
        
        //TODO: make these use program data to define organisation and unit IDs
        channel.QueueBind(queue: _hostQ, exchange: Constants.DISCOVERY_XCH, routingKey: Constants.HOST_RESPONSE_KEY(ClientState._organisationId, ClientState._unitId));
        channel.QueueBind(queue: _authQ, exchange: Constants.DISCOVERY_XCH, routingKey: Constants.AUTH_RESPONSE_KEY(ClientState._organisationId, ClientState._unitId));
    }

    private static void _attachDiscoveryConsumers(IModel channel)
    {
        new AuthConsumer().Attach(channel, _authQ);
        new HostConsumer().Attach(channel, _hostQ);
    }

    private static void _sendDiscoveryMessages()
    {
        
    }

    private static void _bindOperationalQueues(IModel channel)
    {
        
    }

    private static void _attachOperationalConsumers(IModel channel)
    {
        
    }

    private static void _sendHostMessages()
    {
        
    }
}
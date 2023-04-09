using RabbitMQ.Client;
using Vis.Client.Consumers;
using Vis.Common;
using Vis.Common.Models.Messages;
using Vis.Common.Startup;

namespace Vis.Client.Startup;

public class AuthNegotiator : Common.Startup.RabbitMqBaseStartupTask
{
    protected override string _taskDescription { get; } = "Negotiates authorisation with server";
    protected override void _execute()
    {
        var _authQ = _rabbitMqChannel.QueueDeclare();

        if (_authQ is null)
        {
            _TaskFailed("Failed to create 1 or more queues");
        }
        
        _rabbitMqChannel.ExchangeDeclare(exchange: Constants.DISCOVERY_XCH, type: ExchangeType.Topic);
        
        _rabbitMqChannel.QueueBind(
            queue: _authQ, 
            exchange: Constants.DISCOVERY_XCH, 
            routingKey: Constants.AUTH_RESPONSE_KEY(ClientState._organisationId, ClientState._unitId));

        var authConsumer = new AuthConsumer();
        authConsumer.CompletesStartupTask(this);
        authConsumer.Attach(_rabbitMqChannel, _authQ);

        TaskState = State.WAITING;
        
        var authRequest = new AuthRequestMessage(ClientState._organisationId, ClientState._unitId, ClientState._organisationSecret);
        Common.Publishers.SafePublisher.send(authRequest);
        
        _PreventTaskAutoComplete();
    }
}
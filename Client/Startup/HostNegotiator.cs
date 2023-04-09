using RabbitMQ.Client;
using Vis.Client.Consumers;
using Vis.Common;
using Vis.Common.Models.Messages;
using Vis.Common.Startup;

namespace Vis.Client.Startup;

public class HostNegotiator : RabbitMqBaseStartupTask
{
    protected override string _taskDescription { get; } = "Handles finding a host to communicate with directly";
    protected override void _execute()
    {
        var _hostQ = _rabbitMqChannel.QueueDeclare();
        var _createQ = _rabbitMqChannel.QueueDeclare();
        var _inQ = _rabbitMqChannel.QueueDeclare();
        var _outQ= _rabbitMqChannel.QueueDeclare();
        
        if (_createQ is null || _inQ is null || _outQ is null || _hostQ is null)
        {
            _TaskFailed("Failed to create 1 or more queues");
        }
        
        var hostConsumer = new HostConsumer();
        hostConsumer.CompletesStartupTask(this);
        hostConsumer.Attach(_rabbitMqChannel, _hostQ);
        
        _rabbitMqChannel.ExchangeDeclare(exchange: ClientState._organisationExchangeName, type: ExchangeType.Topic);
        
        //TODO: replace with specific client data
        
        _rabbitMqChannel.QueueBind(
            queue: _createQ, 
            exchange: ClientState._organisationExchangeName, 
            routingKey: $"{ClientState._organisationId}.create");
        
        _rabbitMqChannel.QueueBind(
            queue: _inQ,     
            exchange: ClientState._organisationExchangeName, 
            routingKey: $"{ClientState._organisationId}.in");
        
        _rabbitMqChannel.QueueBind(queue: _outQ,    
            exchange: ClientState._organisationExchangeName, 
            routingKey: $"{ClientState._organisationId}.out");
        
        _rabbitMqChannel.QueueBind(
            queue: _hostQ, 
            exchange: Constants.DISCOVERY_XCH, 
            routingKey: Constants.HOST_RESPONSE_KEY(ClientState._organisationId, ClientState._unitId));
        
        new CreateConsumer()
            .Attach(_rabbitMqChannel, _createQ);
        new InConsumer()
            .Attach(_rabbitMqChannel, _inQ);
        new OutConsumer()
            .Attach(_rabbitMqChannel, _outQ);
        
        TaskState = State.WAITING;
        
        var hostRequest = new HostRequestMessage(ClientState._organisationId, ClientState._unitId);
        Common.Publishers.SafePublisher.send(hostRequest);
        
        _PreventTaskAutoComplete();
    }
}
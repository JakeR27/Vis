namespace Vis.Common.Startup;

public class RabbitMqConnector : BaseStartupTask
{
    protected override string _taskDescription { get; } = "Handles making a reusable connection to RabbitMQ";

    private RabbitMqConfigurationParser _configuration = new();
    
    public RabbitMqConnector()
    {
        _configuration.Execute();
        ReadyState = State.COMPLETE;
    }
    protected override void _execute()
    {
        RabbitMq.RmqConnect.Connect(
            _configuration.RabbitMqUsername, 
            _configuration.RabbitMqPassword, 
            _configuration.RabbitMqHostname);
        
        var channel = RabbitMq.RmqConnect.Connection.CreateModel();
        Publishers.SafePublisher.useChannel(channel);
        Logs.LogDebug( "Successfully connected to RMQ and bound channel for sending");
    }
}
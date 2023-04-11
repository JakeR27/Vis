using Vis.Common.Configuration;

namespace Vis.Common.Startup;

public class RabbitMqConfigurationParser : BaseStartupTask
{
    protected override string _taskDescription { get; } =
        "Handles retrieving connection configuration information for RabbitMQ";

    public string RabbitMqUsername { get; private set; }
    public string RabbitMqPassword { get; private set; }
    public string RabbitMqHostname { get; private set; }

    public RabbitMqConfigurationParser()
    {
        ReadyState = State.COMPLETE;
    }
    
    protected override void _execute()
    {
        RabbitMqUsername = new StringConfigurationItem("VIS_RMQ_USERNAME", "backend").Value();
        RabbitMqPassword = new StringConfigurationItem("VIS_RMQ_PASSWORD", "backend").Value();
        RabbitMqHostname = new StringConfigurationItem("VIS_RMQ_HOSTNAME", "ec2-13-42-23-89.eu-west-2.compute.amazonaws.com").Value();
    }
}
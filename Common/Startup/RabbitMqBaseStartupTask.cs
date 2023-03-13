using RabbitMQ.Client;

namespace Vis.Common.Startup;

public abstract class RabbitMqBaseStartupTask : BaseStartupTask
{
    protected IModel _rabbitMqChannel;

    public void Setup(IModel channel)
    {
        _rabbitMqChannel = channel;
        ReadyState = State.COMPLETE;
    }
    
}
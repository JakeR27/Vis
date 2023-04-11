using RabbitMQ.Client;

namespace Vis.Common.Startup;

public abstract class RabbitMqBaseStartupTask : BaseStartupTask
{
    protected IModel _rabbitMqChannel;

    public void Setup(IModel? channel)
    {
        if (channel is null)
        {
            ReadyState = State.ERROR;
            return;
        }
        
        _rabbitMqChannel = channel;
        ReadyState = State.COMPLETE;
    }
    
}
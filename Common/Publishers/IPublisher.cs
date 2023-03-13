namespace Vis.Common;

public interface IPublisher
{
    public void send<TMessage>(string exchange, string routingKey,
        TMessage message);
}
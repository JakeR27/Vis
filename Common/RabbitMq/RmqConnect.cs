using RabbitMQ.Client;

namespace Vis.Common.RabbitMq;

public class RmqConnect
{
    private static ConnectionFactory? _factory;
    public static IConnection Connection;

    public static void Connect(string username, string password, string hostname)
    {
        _factory ??= new ConnectionFactory()
        {
            HostName = hostname,
            UserName = username, Password = password
        };

        Connection = _factory.CreateConnection();
    }
}
using RabbitMQ.Client;

namespace Vis.Common.RabbitMq;

public class RmqConnect
{
    private static ConnectionFactory? _factory;

    public static IConnection Connect(string username, string password)
    {
        _factory ??= new ConnectionFactory()
        {
            HostName = "ec2-13-42-23-89.eu-west-2.compute.amazonaws.com",
            UserName = username, Password = password
        };

        return _factory.CreateConnection();
    }
}
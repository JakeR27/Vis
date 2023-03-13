using Vis.Common.Startup;

namespace Vis.Client.Startup;

public class DatabaseConnector : BaseStartupTask
{
    protected override string _taskDescription { get; } =
        "Handles connecting to the database which records messages if the network connection fails";
    protected override void _execute()
    {
        throw new NotImplementedException();
    }
}
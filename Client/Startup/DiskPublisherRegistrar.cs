using Vis.Common.Startup;

namespace Vis.Client.Startup;

public class DiskPublisherRegistrar : BaseStartupTask
{
    public DiskPublisherRegistrar()
    {
        ReadyState = State.COMPLETE;
    }
    protected override string _taskDescription { get; } =
        "Handles the setup of the system which catches failed messages";
    protected override void _execute()
    {
        Common.Publishers.SafePublisher.useDiskPublisher(new Publishers.DiskPublisher());
    }
}
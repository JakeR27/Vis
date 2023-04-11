using Vis.Common;
using Vis.Common.Configuration;
using Vis.Common.Startup;

namespace Vis.Server.Startup;

public class ConfigurationParser : BaseStartupTask
{
    protected override string _taskDescription { get; } = "Handles loading of configuration settings";

    public ConfigurationParser()
    {
        ReadyState = State.COMPLETE;
    }
    protected override void _execute()
    {
        var debugLevel = new IntegerConfigurationItem("VIS_DEBUG_LEVEL", 1);
        Logs.SetLogLevel(debugLevel.Value());
    }
}
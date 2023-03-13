using Vis.Common;
using Vis.Common.Configuration;
using Vis.Common.Startup;

namespace Vis.Client.Startup;

public class ConfigurationParser : BaseStartupTask
{
    protected override string _taskDescription { get; } = "Handles loading configuration data for client";

    public ConfigurationParser()
    {
        ReadyState = State.COMPLETE;
    }
    protected override void _execute()
    {
        var unitIdEnv = new IntegerConfigurationItem("VIS_UNIT_ID", 10);
        var orgIdEnv = new IntegerConfigurationItem("VIS_ORGANISATION_ID", 1);
        var orgSecEnv = new StringConfigurationItem("VIS_ORGANISATION_SECRET", "DEADBEEF");
        var debugLevel = new IntegerConfigurationItem("VIS_DEBUG_LEVEL", 1);

        ClientState._unitId = unitIdEnv.Value();
        ClientState._organisationId = orgIdEnv.Value();
        ClientState._organisationSecret = orgSecEnv.Value();
        Logs.SetLogLevel(debugLevel.Value());
        
    }
}
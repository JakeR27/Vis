namespace Vis.Common.Startup;

public interface ITaskCompleter
{
    public void CompletesStartupTask(BaseStartupTask task);
}
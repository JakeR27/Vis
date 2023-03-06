namespace Vis.Common.Startup;

public abstract class BaseStartupTask
{
    public State TaskState = State.UNSTARTED;

    protected abstract string _taskDescription { get; }

    public void Execute()
    {
        TaskState = State.STARTED;
        try
        {
            _execute();
            TaskState = State.COMPLETE;
        }
        catch (Exception e)
        {
            TaskState = State.ERROR;
            Logs.LogError($"Startup task failed: {this.GetType()} - {_taskDescription}");
            Logs.LogDebug(e.ToString());
        }
        
        
    }

    protected abstract void _execute();
}
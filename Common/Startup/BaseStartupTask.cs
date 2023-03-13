using System.Diagnostics;

namespace Vis.Common.Startup;

public abstract class BaseStartupTask
{
    public State TaskState = State.UNSTARTED;
    public State ReadyState = State.UNSTARTED;

    private bool _autoComplete = true;

    private string _FriendlyName => GetType().Name; 

    protected BaseStartupTask() {}
    protected abstract string _taskDescription { get; }

    public void Execute()
    {
        
        Logs.LogInfo($"Startup task ({_FriendlyName}) started");
        TaskState = State.STARTED;
        try
        {
            if (ReadyState != State.COMPLETE)
            {
                _TaskFailed("Task was not ready");
            }
            
            _execute();

            if (_autoComplete)
            {
                TaskState = State.COMPLETE;
                Logs.LogInfo($"Startup task ({_FriendlyName}) completed");
            }
        }
        catch (Exception e)
        {
            _fail(e);
        }
    }

    /// <summary>
    /// Forces the task to end and fail
    /// </summary>
    /// <param name="reason">Why the task failed</param>
    /// <exception cref="Exception">Throws an exception with failure detail</exception>
    protected void _TaskFailed(string reason)
    {
        throw new Exception(reason);
    }

    private void _fail(Exception e)
    {
        TaskState = State.ERROR;
        Logs.LogError($"Startup task ({_FriendlyName}) failed - {_taskDescription}");
        Logs.LogDebug(e.Message);
    }

    protected void _PreventTaskAutoComplete()
    {
        _autoComplete = false;
        Logs.LogDebug($"Startup task ({_FriendlyName}) will not complete once function end is reached. Complete with 'ExternalComplete()'");
    }

    public void ExternalComplete()
    {
        TaskState = State.COMPLETE;
        Logs.LogInfo($"Startup task ({_FriendlyName}) completed");
    }

    public void ExternalFail(string message)
    {
        _fail(new Exception(message));
        
    }

    protected abstract void _execute();
}
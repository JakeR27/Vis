using Vis.Common.RabbitMq;
using Timer = System.Timers.Timer;

namespace Vis.Common.Startup;

public class TaskRunner
{
    private static Timer? _timer;
    private Queue<(BaseStartupTask, int)> _startupTasks = new();
    private BaseStartupTask _currentTask;
    public event EventHandler? TaskRunnerCancelled;
    
    public void QueueTask(BaseStartupTask task, int retryAttempts = 1)
    {
        _startupTasks.Enqueue((task, retryAttempts));
    }

    public State Run()
    {
        
        Logs.LogInfo("Starting to run tasks");
        while (_startupTasks.Count > 0)
        {
            var taskState = _runNextTask();
            if (taskState == State.ERROR)
            {
                _timer?.Dispose();
                return State.ERROR;
            }
        }
        _timer?.Dispose();
        return State.COMPLETE;
    }

    private State _runNextTask()
    {
        var (task, retry) = _startupTasks.Dequeue();
        Logs.LogInfo($"Attempting to run ({task.GetType().Name}), will try {retry} times");
        
        var currentAttempts = 0;
        while (currentAttempts <= retry && task.TaskState != State.COMPLETE)
        {
            // If the task is not in a state to be started (eg its already running) skip and wait
            if (task.TaskState is not (State.UNSTARTED or State.ERROR)) continue;
            currentAttempts++;
            _runTask(task);
        }

        if (task.TaskState != State.COMPLETE)
        {
            Logs.LogError("Failed to complete all tasks");
            return State.ERROR;
        }

        Logs.LogInfo($"Finished running ({task.GetType().Name}) with {currentAttempts} attempts");
        return State.COMPLETE;
    }

    private void _runTask(BaseStartupTask task)
    {
        _resetTimer();
        _currentTask = task;
        Logs.LogDebug($"Calling execute on ({task.GetType().Name})");
        (task as RabbitMqBaseStartupTask)?.Setup(RmqConnect.CreateChannel());
        task.Execute();
        
    }
    
    private void _resetTimer()
    {
        _timer?.Dispose();
        _timer = new Timer();
        _timer.Interval = 10_000;
        _timer.Elapsed += _timerOnElapsed;
        _timer.Start();
    }
    
    private void _timerOnElapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        _resetTimer();
        if (_timer != null) _timer.Enabled = false;
        _currentTask.ExternalFail("Failed due to timeout");
        TaskRunnerCancelled?.Invoke(this, EventArgs.Empty);
    }
    
}
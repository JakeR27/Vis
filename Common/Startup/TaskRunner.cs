using Vis.Common.RabbitMq;

namespace Vis.Common.Startup;

public class TaskRunner
{
    private Queue<(BaseStartupTask, int)> _startupTasks = new();

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
            if (taskState == State.ERROR) return State.ERROR;
        }
        return State.COMPLETE;
    }

    private State _runNextTask()
    {
        var (task, retry) = _startupTasks.Dequeue();
        Logs.LogInfo($"Attempting to run ({task.GetType().Name}), will try {retry} times");
        
        var currentAttempts = 0;
        while (currentAttempts <= retry && task.TaskState != State.COMPLETE)
        {
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
        // switch (task)
        // {
        //     case RabbitMqBaseStartupTask rabbitTask:
        //     {
        //         Logs.LogDebug("Executing 'RabbitMqBaseStartupTask'");
        //         (task as RabbitMqBaseStartupTask)?.Setup(RmqConnect.Connection.CreateModel());
        //         task.Execute();
        //         break;
        //     }  
        //     case BaseStartupTask baseTask:
        //     {
        //         Logs.LogDebug("Executing 'BaseStartupTask'");
        //         task.Execute();
        //         break;
        //     }
        //     
        // }
        Logs.LogDebug($"Calling execute on ({task.GetType().Name})");
        (task as RabbitMqBaseStartupTask)?.Setup(RmqConnect.Connection.CreateModel());
        task.Execute();
        
    }
}
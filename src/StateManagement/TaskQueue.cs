using System;
using System.Collections.Generic;

public class TaskQueue
{
    private List<ITask> queue;

    public TaskQueue()
    {
        queue = new List<ITask>();
    }

    public void Add(ITask Task)
    {
        queue.Add(Task);
    }

    public void Process()
    {
        if (queue.Count > 0 && queue[0].CanExecute())
        {
            queue[0].Execute();
            queue.Remove(queue[0]);
        }
    }

    internal void Clear()
    {
        queue = new List<ITask>();
    }
}
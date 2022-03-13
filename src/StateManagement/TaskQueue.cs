using System;
using System.Collections.Generic;

public class TaskQueue
{
    private Dictionary<string, ITask> dict;
    public TaskQueue()
    {
        dict = new Dictionary<string, ITask>();
    }

    public void Add(ITask Task, string key)
    {
        dict[key] = Task;
    }

    public void Process()
    {
        if (dict.Count > 0)
        {
            foreach (var key in dict.Keys)
            {
                if (dict[key].CanExecute())
                {
                    dict[key].Execute();
                }
            }
        }
    }

    internal void Clear()
    {
        dict = new Dictionary<string, ITask>();
    }
}
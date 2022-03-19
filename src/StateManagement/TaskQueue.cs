using System.Linq;
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
        var keysToRemove = new List<string>();
        if (dict.Count > 0)
        {
            foreach (var key in dict.Keys)
            {
                if (dict[key].CanExecute())
                {
                    if (dict[key].Execute())
                    {
                        keysToRemove.Add(key);
                    }
                }
                else if (dict[key].WhenCannotExecute())
                {
                    keysToRemove.Add(key);
                }
            }
            foreach (var key in keysToRemove)
            {
                if (dict.Keys.Contains(key))
                {
                    dict.Remove(key);
                }
            }
        }
    }

    internal void Clear()
    {
        dict = new Dictionary<string, ITask>();
    }
}
using System.Collections.Generic;
using System.Linq;

public class ContinuousTaskHandler
{
    private List<ContinuousTask> Tasks;

    public ContinuousTaskHandler()
    {
        this.Tasks = new List<ContinuousTask>();

    }

    public void Add(ContinuousTask Task)
    {
        this.Tasks.Add(Task);
    }



    public void Process()
    {
        foreach (var Task in Tasks)
        {
            Task.Process();
        }
        Tasks = Tasks.Where(t => !t.complete).ToList();
    }
}
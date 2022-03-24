using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DoNothingTask : ITask
{
    public IEnumerable<IOrder> Orders { get; set; }

    public bool Execute()
    {
        return true;
    }

    public bool IsComplete()
    {
        return false;
    }

}

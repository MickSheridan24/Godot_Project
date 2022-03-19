using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DoNothingTask : ITask
{
    public bool CanExecute()
    {
        return true;
    }

    public bool Execute()
    {
        return true;
    }
    public bool WhenCannotExecute()
    {
        return true;
    }
}

using System.Collections.Generic;

public interface ITask
{
    IEnumerable<IOrder> Orders { get; set; }
    bool Execute();
    bool IsComplete();
}
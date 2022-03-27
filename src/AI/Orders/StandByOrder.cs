
using System;

public class StandByOrder : IOrder
{
    private Func<bool> condition;

    public StandByOrder(Func<bool> condition)
    {
        this.condition = condition;
    }

    public StandByOrder()
    {
        this.condition = () => true;
    }

    public bool Execute()
    {
        return condition();
    }
}

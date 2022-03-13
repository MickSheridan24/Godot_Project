using System.Collections.Generic;

public class CombinedOrder : IOrder
{
    private List<IOrder> orders;

    public CombinedOrder(List<IOrder> orders)
    {
        this.orders = orders;
    }
    public bool Execute()
    {
        foreach (var o in orders)
        {
            o.Execute();
        }
        return true;
    }
}
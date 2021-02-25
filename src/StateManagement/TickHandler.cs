
using System.Collections.Generic;
using System.Linq;

public class TickHandler
{
    private int frames = 10;

    private List<TickOrder> orders;
    public TickHandler()
    {
        orders = new List<TickOrder>();
    }

    public void AddOrder(IOrder order, double v)
    {
        orders.Add(new TickOrder { order = order, ticks = v, defaultTicks = v, complete = false });
    }

    public void AddOrder(TickOrder order)
    {
        orders.Add(order);
    }
    public void Tick()
    {
        frames--;
        if (frames <= 0)
        {
            ResetTick();
        }
    }

    internal void Refresh(TickOrder order)
    {
        var foundOrder = orders.Find(o => o == order);
        if (foundOrder != null)
        {
            foundOrder.complete = false;
            foundOrder.ticks = foundOrder.defaultTicks;
        }
        else
        {
            order.complete = false;
            order.ticks = order.defaultTicks;
            orders.Add(order);
        }
    }

    internal void Remove(TickOrder order)
    {
        var foundOrder = orders.Find(o => o == order);
        if (foundOrder != null)
        {
            orders.Remove(foundOrder);
        }
    }

    private void ResetTick()
    {
        frames = 10;
        for (var i = 0; i < orders.Count; i++)
        {
            orders[i].Tick();
        }

        orders = orders.Where(o => !o.complete).ToList();
    }
}

public class TickOrder
{
    public IOrder order { get; set; }
    public double ticks { get; set; }
    public double defaultTicks { get; set; }

    public bool complete;

    public void Tick()
    {
        ticks--;
        if (ticks <= 0)
        {
            order.Execute();
            complete = true;
        }
    }
}


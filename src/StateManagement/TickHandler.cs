
using System.Collections.Generic;
using System.Linq;

public class TickHandler
{
    private int frames = 60;

    private List<TickOrder> orders;
    public TickHandler()
    {
        orders = new List<TickOrder>();
    }

    public void AddOrder(IOrder order, int v)
    {
        orders.Add(new TickOrder { order = order, ticks = v, complete = false });
    }

    public void Tick()
    {
        frames--;
        if (frames <= 0)
        {
            ResetTick();
        }
    }

    private void ResetTick()
    {
        frames = 60;
        for (var i = 0; i < orders.Count; i++)
        {
            orders[i].Tick();
        }

        orders = orders.Where(o => !o.complete).ToList();
    }

    private class TickOrder
    {
        public IOrder order { get; set; }
        public int ticks { get; set; }

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

}


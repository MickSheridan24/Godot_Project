using System;
using Godot;

public class ContinuousTask
{
    public Func<bool> condition { get; set; }
    public TickOrder order { get; set; }
    public TickHandler tickHandler { get; set; }

    public bool complete;

    public ContinuousTask(Func<bool> condition, TickOrder order, TickHandler handler)
    {
        this.condition = condition;
        this.order = order;
        this.tickHandler = handler;
        complete = false;
    }

    public void Process()
    {

        if (condition())
        {
            if (order.complete)
            {
                tickHandler.Refresh(order);
            }
        }
        else
        {
            complete = true;
        }
    }
}
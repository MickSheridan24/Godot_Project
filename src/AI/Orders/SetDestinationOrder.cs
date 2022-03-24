using System;
using Godot;
public class SetDestinationOrder : IOrder
{
    public IMove m { get; set; }
    public Vector2 d { get; set; }

    private Func<bool> condition;

    public SetDestinationOrder(IMove m, Vector2 d, Func<bool> condition)
    {
        this.m = m;
        this.d = d;
        this.condition = condition;
    }
    public SetDestinationOrder(IMove m, Vector2 d)
    {
        this.m = m;
        this.d = d;
        this.condition = () => true;
    }

    public bool Execute()
    {
        if (condition())
        {
            m.destination = d;
            return true;
        }
        return false;
    }
}

using Godot;
public class SetDestinationOrder : IOrder
{
    public IMove m { get; set; }
    public Vector2 d { get; set; }
    public SetDestinationOrder(IMove m, Vector2 d)
    {
        this.m = m;
        this.d = d;
    }

    public bool Execute()
    {
        m.destination = d;
        return true;
    }
}

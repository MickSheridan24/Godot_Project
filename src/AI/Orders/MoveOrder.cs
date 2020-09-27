using Godot;
public class MoveOrder : IOrder
{
    public IMove m { get; set; }

    public float delta { get; set; }
    public MoveOrder(IMove m, float d)
    {
        this.m = m;
        this.delta = d;
    }

    public bool Execute()
    {
        m.HandleMove(delta);
        return true;
    }
}

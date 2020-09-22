using Godot;
public class MoveOrder : IOrder
{
    public IMove m { get; set; }
    public MoveOrder(IMove m)
    {
        this.m = m;
    }

    public bool Execute()
    {
        m.HandleMove();
        return true;
    }
}

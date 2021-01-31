
using Godot;

public class SpawnOrder : IOrder
{
    private BaseActorNode node;
    private Vector2 pos;
    private Node2D parent;

    public SpawnOrder(BaseActorNode node, Node2D parent, Vector2 pos)
    {
        this.node = node;
        this.pos = pos;
        this.parent = parent;
    }

    public bool Execute()
    {
        GD.Print("CREATING NODE");
        parent.AddChild(node);
        node.Position = pos;
        node.Activate();


        return true;
    }


}
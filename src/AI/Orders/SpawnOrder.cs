
using Godot;

public class SpawnOrder : IOrder
{
    private BaseActorNode node;
    private Node2D parent;

    private IHaveSpawnArea spawnArea;

    public SpawnOrder(BaseActorNode node, Node2D parent, IHaveSpawnArea spawnArea)
    {
        this.node = node;
        this.parent = parent;
        this.spawnArea = spawnArea;
    }

    public bool Execute()
    {

        GD.Print("FINDING SPACE FOR NODE");
        var pos = spawnArea.FindOpenPosition();
        if (pos == null)
        {
            GD.Print("NO SPOT FOR NODE");
            return false;
        }
        else
        {
            parent.AddChild(node);
            node.Position = pos ?? Vector2.Zero;
            node.Activate();
            return true;
        }

    }


}

using Godot;

public class SpawnOrder<T> : IOrder where T : BaseActorNode
{
    private BaseActorNode node;
    private Node2D parent;

    private IHaveSpawnArea spawnArea;

    public SpawnOrder(T node, Node2D parent, IHaveSpawnArea spawnArea)
    {
        this.node = node;
        this.parent = parent;
        this.spawnArea = spawnArea;
    }

    public bool Execute()
    {

        GD.Print("FINDING SPACE FOR NODE");
        var size = node is IHaveSize ? (node as IHaveSize).size : Vector2.Zero;
        var pos = spawnArea.FindOpenPosition(size);
        if (pos == null)
        {
            GD.Print("NO SPOT FOR NODE");
            return false;
        }
        else
        {
            parent.AddChild(node);
            node.InitiatePosition(pos ?? Vector2.Zero);
            node.Activate();
            return true;
        }

    }


}
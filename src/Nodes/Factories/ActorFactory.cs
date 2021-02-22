using Godot;

public class ActorFactory
{

    private static PackedScene snNPCNode => (PackedScene)ResourceLoader.Load("res://scenes/NPC.tscn");
    public static NPC CreateNPC(IHaveRuntime parent)
    {
        var node = snNPCNode.Instance() as NPC;
        node.state = new NPCState(node, parent, false);
        parent.runtime.entityRegistry.Add(node);
        return node;
    }
}
using Godot;

public class NPCState : BaseActorState
{
    public NPCState(NPC node) : base(node, node.runtime.playerState)
    {
        speed = Stat.Speed(115);
        health = Stat.Health(100);
    }

    public NPCState(NPC node, IHaveRuntime parent) : base(node, parent, parent.runtime.playerState)
    {
        speed = Stat.Speed(115);
        health = Stat.Health(100);
    }
}

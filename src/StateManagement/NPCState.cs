using Godot;

public class NPCState : BaseActorState
{
    public NPCState(NPC node, bool debug = false) : base(node, node.runtime.playerState, debug)
    {
        speed = Stat.Speed(115);
        health = Stat.Health(100);
    }

    public NPCState(NPC node, IHaveRuntime parent, bool debug = false) : base(node, parent, parent.runtime.playerState, debug)
    {
        speed = Stat.Speed(115);
        health = Stat.Health(100);
    }
}

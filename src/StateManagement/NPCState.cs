using Godot;

public class NPCState : BaseActorState
{
    public NPCState(NPC node) : base(node)
    {
        speed = Stat.Speed(115);
        health = Stat.Health(100);
    }

    public NPCState(NPC node, IHaveRuntime parent) : base(node, parent)
    {
        speed = Stat.Speed(115);
        health = Stat.Health(100);
    }
}

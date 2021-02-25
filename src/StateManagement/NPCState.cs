using Godot;

public class NPCState : BaseActorState
{

    public Stat damage { get; set; }
    public NPCState(NPC node, bool debug = false) : base(node, node.runtime.playerState, debug)
    {
        speed = Stat.Speed(115);
        health = Stat.Health(10000);
        damage = Stat.Damage(50);

    }

    public NPCState(NPC node, IHaveRuntime parent, bool debug = false) : base(node, parent, parent.runtime.playerState, debug)
    {
        speed = Stat.Speed(115);
        health = Stat.Health(10000);
        damage = Stat.Damage(50);
    }
}

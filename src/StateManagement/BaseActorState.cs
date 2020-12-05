
using Godot;

public class BaseActorState
{
    public Stat speed { get; set; }
    public Stat health { get; set; }
    public TickHandler tickHandler { get; }
    public StatusHandler statusHandler { get; set; }
    public ElevationHandler elevationHandler { get; set; }
    public Node2D node { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }


    public BaseActorState(Node2D node)
    {
        this.node = node;
        tickHandler = new TickHandler();
        statusHandler = new StatusHandler(node as ISufferStatusEffects);
        elevationHandler = new ElevationHandler(node as IElevatable, (node as IHaveRuntime).runtime);
        (node as BaseActorNode).state = this;

    }


    public bool HandleDamage(int damage)
    {
        health.current -= damage;
        return health.current > 0;
    }
    public void AddStatus(eStatusEffect s, int duration)
    {
        statusHandler.AddStatus(StatusEffect.Create(s));
        tickHandler.AddOrder(new RemoveStatusOrder(node as ISufferStatusEffects, s), duration);
    }
}


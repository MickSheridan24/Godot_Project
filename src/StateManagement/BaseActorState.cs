
using Godot;

public class BaseActorState
{
    public Stat speed { get; set; }
    public Stat health { get; set; }
    public TickHandler tickHandler { get; private set; }
    public StatusHandler statusHandler { get; set; }
    public ElevationHandler elevationHandler { get; set; }

    public TaskQueue taskQueue { get; private set; }

    public PlayerState player { get; set; }

    public ContinuousTaskHandler continuousActionHandler;

    public Node2D node { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public TargetingSystem Targeting { get; internal set; }

    private Runtime runtime;

    public BaseActorState(Node2D node, PlayerState player)
    {
        runtime = (node as IHaveRuntime).runtime;

        Config(node, player);
    }

    public BaseActorState(Node2D node, IHaveRuntime parent, PlayerState player)
    {
        runtime = parent.runtime;
        Config(node, player);

    }

    private void Config(Node2D node, PlayerState player)
    {
        this.player = player;
        this.node = node;
        tickHandler = new TickHandler();
        statusHandler = new StatusHandler(node as ISufferStatusEffects);
        elevationHandler = new ElevationHandler(node as IElevatable, runtime);
        Targeting = new TargetingSystem();
        taskQueue = new TaskQueue();
        continuousActionHandler = new ContinuousTaskHandler();
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

    public void Tick()
    {
        tickHandler.Tick();
    }
}


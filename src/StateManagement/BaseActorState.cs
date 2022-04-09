
using Godot;

public class BaseActorState
{
    public Stat speed { get; set; }
    public Stat health { get; set; }

    public Stat range { get; set; }
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

    public Runtime runtime;

    public BaseActorState(Node2D node, PlayerState player, bool debug = false)
    {
        runtime = (node as IHaveRuntime).runtime;

        Config(node, player, debug);
    }

    public BaseActorState(Node2D node, IHaveRuntime parent, PlayerState player, bool debug = false)
    {
        runtime = parent.runtime;
        Config(node, player, debug);
    }

    private void Config(Node2D node, PlayerState player, bool debug = false)
    {
        this.player = player;
        this.node = node;
        (node as BaseActorNode).debug = debug;
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
        health.FlatChange(-1 * damage);
        return health.current > 0;
    }
    public void AddStatus(eStatusEffect s, double duration)
    {
        statusHandler.AddStatus(StatusEffect.Create(s));
        tickHandler.AddOrder(new RemoveStatusOrder(node as ISufferStatusEffects, s), duration);
    }

    public void Tick()
    {
        tickHandler.Tick();
    }
}


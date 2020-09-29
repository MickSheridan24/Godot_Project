using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class EnemyState
{
    public IAI ai { get; set; }
    public Enemy node { get; set; }
    public bool isClimbing;

    public Stat health;
    public Stat speed;
    public ElevationHandler elevationHandler;
    public StatusHandler statusHandler;
    private TickHandler tickHandler;

    public EnemyState(IAI AI, Enemy enemy)
    {
        isClimbing = false;
        ai = AI;
        node = enemy;
        health = Stat.Health(400);
        speed = Stat.Speed(85);

        elevationHandler = new ElevationHandler(node, node.runtime);
        tickHandler = new TickHandler();
        statusHandler = new StatusHandler(node);
    }

    public void Tick()
    {
        tickHandler.Tick();
    }

    public IOrder RequestAction(float d)
    {
        return ai.Request(this, d);
    }

    public bool HandleDamage(int d)
    {
        health.current -= d;
        return health.current > 0;
    }

    internal bool CanMove()
    {
        return !statusHandler.HasStatus(eStatusEffect.JOLTED);
    }

    internal void InitClimbing(eCollisionLayers level)
    {
        isClimbing = true;
        tickHandler.AddOrder(new ClimbOrder(node, level), 5);
    }

    internal void StopClimbing()
    {
        isClimbing = false;
    }

    internal void DisableFall(int v)
    {
        node.isFallDisabled = true;
        tickHandler.AddOrder(new ReEnableFallOrder(node), v);
    }

    internal void AddStatus(eStatusEffect s, int duration)
    {
        statusHandler.AddStatus(StatusEffect.Create(s));
        tickHandler.AddOrder(new RemoveStatusOrder(node, s), duration);

    }
}
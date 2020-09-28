using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class EnemyState
{
    public IAI ai { get; set; }
    public Enemy node { get; set; }

    public int health { get; set; }

    private List<IStatusEffect> statuses;
    internal int maxHealth;

    public ElevationHandler elevationHandler;

    private TickHandler tickHandler;

    public bool isClimbing;



    public EnemyState(IAI AI, Enemy enemy)
    {
        isClimbing = false;
        ai = AI;
        node = enemy;
        health = 400;
        maxHealth = 400;
        statuses = new List<IStatusEffect>();
        elevationHandler = new ElevationHandler(node, node.runtime);
        tickHandler = new TickHandler();
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
        health -= d;
        return health > 0;
    }

    internal void HandleStatuses()
    {
        foreach (var status in statuses)
        {
            status.Enact(node);
            status.Reduce();
        }
        statuses = statuses.Where(s => s.duration > 0).ToList();
    }


    private IStatusEffect GetStatus(eStatusEffect effect)
    {
        return statuses.Where(s => s.type == effect).FirstOrDefault();
    }

    public bool HasStatus(eStatusEffect effect)
    {
        return statuses.Where(s => s.type == effect).FirstOrDefault() != null;
    }


    internal void AddStatusEffect(IStatusEffect effect)
    {
        var presentStatus = GetStatus(effect.type);
        if (presentStatus != null)
        {
            presentStatus.Increase(effect.duration);
        }
        else
        {
            statuses.Add(effect);
        }

    }

    internal bool CanMove()
    {
        return !HasStatus(eStatusEffect.JOLTED);
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
}
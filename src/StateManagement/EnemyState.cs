using System;
using System.Collections.Generic;
using System.Linq;

public class EnemyState
{
    public IAI ai { get; set; }
    public Enemy node { get; set; }

    public int health { get; set; }

    private List<IStatusEffect> statuses;
    internal int maxHealth;

    public EnemyState(IAI AI, Enemy enemy)
    {
        ai = AI;
        node = enemy;
        health = 400;
        maxHealth = 400;
        statuses = new List<IStatusEffect>();
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
}
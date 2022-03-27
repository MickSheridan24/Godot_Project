using System;
using System.Collections.Generic;
using Godot;

public class SmartZombieAI : IAI
{
    public BaseActorNode node { get; set; }
    public IMove move { get; }
    public BaseActorState state;
    private ITarget finalTarget;
    private float range;
    private AttackTask task;
    private ITarget currentTarget;

    public SmartZombieAI(Enemy node, ITarget target, float range)
    {
        this.node = node;
        this.state = node.state;
        this.finalTarget = target;
        this.move = node as IMove;
        this.range = range;
        this.currentTarget = target;
        this.task = new AttackTask(node as ICanAttack, target as IDamageable);
    }

    public IOrder Request(EnemyState enemyState, float d)
    {
        //task is null
        if (task == null)
        {
            if (finalTarget == null || finalTarget.IsFreed())
            {
                finalTarget = findNewDistraction();
            }
            else
            {
                task = new AttackTask(node as ICanAttack, finalTarget as IDamageable);
            }
        }
        //Was I able to attack?
        else
        {
            if (task.IsComplete())
            {
                task = null;
            }
            else
            {
                task.Execute();
                if (task.IsComplete())
                {
                    task = null;
                }
                else
                {
                    // Can I be distracted
                    if (!node.ToLocal(currentTarget.GetTargetPosition()).WithinRange(node.Position, new Vector2(75, 75)))
                    {
                        var distraction = findNewDistraction();
                        if (distraction != null && distraction != currentTarget && distraction is IDamageable)
                        {
                            currentTarget = distraction;
                            task = new AttackTask(this.node as ICanAttack, currentTarget as IDamageable);
                        }
                    }
                }
            }



        }
        return new StandByOrder();
    }

    private ITarget findNewDistraction()
    {
        var targets = state?.runtime?.entityFinder?.FindMinions(node.GlobalPosition, range.ToVector()) ?? new List<NPC>();

        if (targets.Count > 0)
        {
            return targets[0];
        }
        return null;
    }
}
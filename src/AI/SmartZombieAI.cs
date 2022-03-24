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
            if (finalTarget.IsFreed())
            {
                finalTarget = findNewDistraction();
            }
            else
            {
                task = new AttackTask(this as ICanAttack, finalTarget as IDamageable);
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
                    //Can I be distracted
                    if (!node.ToLocal(currentTarget.GetTargetPosition()).WithinRange(node.Position, new Vector2(75, 75)))
                    {
                        var distraction = findNewDistraction();
                        if (distraction != null && distraction is IDamageable)
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



    // public IOrder Request(EnemyState enemyState, float d)
    // {
    //     if (finalTarget.IsFreed() || finalTarget == null)
    //     {
    //         return new StandByOrder();
    //     }
    //     else if (state != null)
    //     {
    //         if (currentTarget == null || currentTarget.IsFreed())
    //         {
    //             //Try to find new Distraction 
    //             currentTarget = findNewDistraction();
    //             if (currentTarget != null)
    //             {
    //                 //Success, set destination
    //                 state.taskQueue.Add(currentTarget.GetHostileTask(node), "ATTACK");
    //                 var setDest = new SetDestinationOrder(move, currentTarget.GetTargetPosition());
    //                 var moveOrd = new MoveOrder(move, d);
    //                 return new CombinedOrder(new List<IOrder> { setDest, moveOrd });
    //             }
    //             else if (move.destination != finalTarget.GetTargetPosition())
    //             {
    //                 //Failure set final target
    //                 state.taskQueue.Add(finalTarget.GetHostileTask(node), "ATTACK");
    //                 var setDest = new SetDestinationOrder(move, finalTarget.GetTargetPosition());
    //                 var moveOrd = new MoveOrder(move, d);
    //                 return new CombinedOrder(new List<IOrder> { setDest, moveOrd });
    //             }
    //             else if (!move.GlobalPosition.WithinRange(finalTarget.GetTargetPosition(), state.range.current.ToVector()))
    //             {
    //                 //Failure, continue towards final target
    //                 var setDest = new SetDestinationOrder(move, finalTarget.GetTargetPosition());
    //                 var moveOrd = new MoveOrder(move, d);
    //                 return new CombinedOrder(new List<IOrder> { setDest, moveOrd });
    //             }
    //         }
    //         else if (!move.GlobalPosition.WithinRange(currentTarget.GetTargetPosition(), state.range.current.ToVector()))
    //         {
    //             var setDest = new SetDestinationOrder(move, currentTarget.GetTargetPosition());
    //             var moveOrd = new MoveOrder(move, d);
    //             return new CombinedOrder(new List<IOrder> { setDest, moveOrd });
    //         }

    //     }
    //     return new StandByOrder();
    // }

    private ITarget findNewDistraction()
    {
        var targets = state?.runtime?.entityFinder?.FindMinions(move.GlobalPosition, range.ToVector()) ?? new List<NPC>();

        if (targets.Count > 0)
        {
            return targets[0];
        }
        return null;
    }
}
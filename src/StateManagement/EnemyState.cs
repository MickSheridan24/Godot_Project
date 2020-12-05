using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class EnemyState : BaseActorState
{
    public IAI ai { get; set; }
    public bool isClimbing;
    public Stat damage;



    public EnemyState(IAI AI, Enemy enemy) : base(enemy)
    {
        isClimbing = false;
        ai = AI;
        health = Stat.Health(400);
        speed = Stat.Speed(85);
        damage = Stat.Damage(50);
    }



    public IOrder RequestAction(float d)
    {
        return ai.Request(this, d);
    }


    internal bool CanMove()
    {
        return !statusHandler.HasStatus(eStatusEffect.JOLTED);
    }

    internal void InitClimbing(eCollisionLayers level)
    {
        isClimbing = true;
        tickHandler.AddOrder(new ClimbOrder(node as IElevatable, level), 5);
    }

    internal void StopClimbing()
    {
        isClimbing = false;
    }

    internal void DisableFall(int v)
    {
        (node as IElevatable).isFallDisabled = true;
        tickHandler.AddOrder(new ReEnableFallOrder(node as IElevatable), v);
    }
}
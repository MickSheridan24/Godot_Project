using System.Linq;
using System.Collections.Generic;
using Godot;

public class AttackTask : ITask
{


    private ICanAttack actor;
    private IDamageable enemy;
    private TickOrder cooldownTick;

    private Vector2 localEnemyPos => actor.ToLocal(enemy.GlobalPosition);

    public IEnumerable<IOrder> Orders { get; set; }

    private TickHandler tickHandler => actor.state.tickHandler;


    public AttackTask(ICanAttack actor, IDamageable enemy)
    {
        this.actor = actor;
        this.enemy = enemy;

        cooldownTick = new TickOrder()
        {
            order = new StandByOrder(() => true),
            ticks = 10,
            defaultTicks = 10,
            complete = true
        };


        Orders = new List<IOrder>()
        {
            new StandByOrder(() => !cooldownTick.complete),
            new AttackOrder(actor, enemy, CanExecute),
            new SetDestinationOrder(actor as IMove, actor.ToLocal(enemy.GlobalPosition), () => actor is IMove)
        };

        new SetDestinationOrder(actor as IMove, actor.ToLocal(enemy.GlobalPosition), () => actor is IMove).Execute();

    }

    public bool Execute()
    {
        return Orders.Any(o => o.Execute());
    }

    public bool IsComplete()
    {
        return enemy.IsFreed();
    }

    private bool CanExecute()
    {
        var area = (enemy as ITarget).GetTargetArea();
        actor.RayCast.CastTo = actor.Range * actor.Position.DirectionTo(actor.ToLocal(enemy.GlobalPosition));
        actor.RayCast.CollideWithAreas = true;
        var collider = actor.RayCast.GetCollider();

        if (!enemy.IsFreed() && collider != null && collider == area)
        {
            ResetAttackCooldown();
            return true;
        }
        return false;
    }

    private void ResetAttackCooldown()
    {
        cooldownTick = new TickOrder()
        {
            order = new StandByOrder(() => true),
            ticks = 15,
            defaultTicks = 15,
            complete = false
        };
        tickHandler.AddOrder(cooldownTick);
    }

}
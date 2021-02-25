using Godot;

public class AttackTask : ITask
{


    private ICanAttack actor;
    private IDamageable enemy;

    public AttackTask(ICanAttack actor, IDamageable enemy)
    {
        this.actor = actor;
        this.enemy = enemy;
    }
    public bool CanExecute()
    {
        return enemy.Position.WithinRange(actor.Position, new Vector2(actor.Range, actor.Range));
    }

    public void Execute()
    {
        var attackOrder = new AttackOrder(actor, enemy, CanExecute);

        var tickOrder = new TickOrder()
        {
            order = attackOrder,
            ticks = 9,
            defaultTicks = 9,
            complete = false
        };

        var continuousTask = new ContinuousTask(() => CanExecute(), tickOrder, actor.state.tickHandler);
        actor.state.tickHandler.AddOrder(tickOrder);
        actor.state.continuousActionHandler.Add(continuousTask);
    }
}
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
        return !enemy.IsFreed() && enemy.Position.WithinRange(actor.Position, new Vector2(actor.Range, actor.Range));
    }

    public bool WhenCannotExecute()
    {
        if (enemy.IsFreed())
        {
            return true;
        }
        else if (actor is IMove)
        {
            (actor as IMove).destination = enemy.Position;
            return false;
        }
        else
        {
            return false;
        }
    }

    public bool Execute()
    {
        if (!enemy.IsFreed())
        {
            new AttackOrder(actor, enemy, () => true).Execute();
            return false;
        }
        else
        {
            return true;
        }
    }
}
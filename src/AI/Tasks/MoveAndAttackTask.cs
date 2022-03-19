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

    public void WhenCannotExecute()
    {

    }

    public void Execute()
    {

        var attackOrder = new AttackOrder(actor, enemy, CanExecute);

    }
}
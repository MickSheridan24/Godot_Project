using System;

public class AttackOrder : IOrder
{
    private ICanAttack attacker;
    private IDamageable target;
    private Func<bool> condition;

    public AttackOrder(ICanAttack attacker, IDamageable target, Func<bool> condition)
    {
        this.attacker = attacker;
        this.target = target;
        this.condition = condition;
    }
    public bool Execute()
    {
        if (condition())
        {
            target.Damage(attacker.CurrentDamage, attacker.damageType);
            return true;
        }
        else return false;

    }
}
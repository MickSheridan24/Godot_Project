public class AttackOrder : IOrder
{
    private ICanAttack attacker;
    private IDamageable target;

    public AttackOrder(ICanAttack attacker, IDamageable target)
    {
        this.attacker = attacker;
        this.target = target;
    }
    public bool Execute()
    {
        target.Damage(attacker.CurrentDamage, attacker.damageType);
        return true;
    }
}
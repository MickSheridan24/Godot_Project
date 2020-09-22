using System;

public class EnemyState
{
    public IAI ai { get; set; }
    public Enemy node { get; set; }

    public int health { get; set; }
    public EnemyState(IAI AI, Enemy enemy)
    {
        ai = AI;
        node = enemy;
        health = 400;
    }

    public IOrder RequestAction()
    {
        return ai.Request(this);
    }

    public bool HandleDamage(int d)
    {
        health -= d;
        return health > 0;
    }


}
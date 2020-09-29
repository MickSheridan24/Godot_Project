
using Godot;

public class ZombieAI : IAI
{


    public IMove node { get; set; }

    public ITarget target { get; set; }

    public ZombieAI(IMove node, ITarget target)
    {
        this.node = node;
        this.target = target;
    }

    public IOrder Request(EnemyState enemyState, float d)
    {
        if ((node as Node).IsFreed() || node == null)
        {
            return new StandByOrder();
        }
        if (node.destination == null || !node.destination.WithinAreaOf(target.GetTargetPosition(), new Vector2(30, 30)))
        {
            return new SetDestinationOrder(node, target.GetTargetPosition());
        }
        else
        {
            return new MoveOrder(node, d);
        }
    }
}

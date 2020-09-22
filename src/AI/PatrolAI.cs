using System;
using Godot;
public class PatrolAI : IAI
{
    public PatrolAI(IMove node)
    {
        this.node = node;

    }
    public IMove node { get; set; }

    private Vector2 dir;
    private Vector2 diff;
    private bool flip;

    public PatrolAI(IMove node, Vector2 dir, Vector2 diff)
    {
        this.node = node;
        this.dir = dir;
        this.diff = diff;
        flip = false;
    }

    public IOrder Request(EnemyState enemyState)
    {
        if (node.destination != node.Position && node.CanMove())
        {
            return new MoveOrder(node);
        }
        else if (node.CanMove())
        {
            return new SetDestinationOrder(node, GetNewDest());
        }
        else
        {
            return new StandByOrder();
        }
    }

    private Vector2 GetNewDest()
    {
        flip = !flip;
        var flipV = flip ? new Vector2(-1, -1) : new Vector2(1, 1);

        return node.Position + (diff * flipV * dir);
    }
}
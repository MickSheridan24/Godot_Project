using System;
using Godot;
public class PatrolAI : IAI
{

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

    public IOrder Request(EnemyState enemyState, float d)
    {
        if (node.destination != node.GlobalPosition && node.CanMove())
        {
            return new MoveOrder(node, d);
        }
        else if (node.CanMove())
        {
            return new SetDestinationOrder(node, GetNewDest(), () => true);
        }
        else
        {
            return new StandByOrder(() => true);
        }
    }

    private Vector2 GetNewDest()
    {
        flip = !flip;
        var flipV = flip ? new Vector2(-1, -1) : new Vector2(1, 1);

        return node.GlobalPosition + (diff * flipV * dir);
    }
}
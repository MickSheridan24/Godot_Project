using Godot;

public class VectorTarget : ITarget
{
    private Vector2 position { get; set; }
    public bool MovingTarget { get; set; }

    public eTeam Team => eTeam.NEUTRAL;

    public VectorTarget(Vector2 v)
    {
        position = v;
        MovingTarget = false;
    }

    public Vector2 GetTargetPosition()
    {
        return position;
    }

    public bool IsFreed()
    {
        return false;
    }

    public ITask GetFriendlyTask(BaseActorNode node)
    {
        throw new System.NotImplementedException();
    }

    public ITask GetHostileTask(BaseActorNode node)
    {
        throw new System.NotImplementedException();
    }

    public void HighlightTarget()
    {
    }

    public void DeHighlightTarget()
    {
    }
}
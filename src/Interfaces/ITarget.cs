using Godot;

public interface ITarget
{
    bool MovingTarget { get; }
    Vector2 GetTargetPosition();
    bool IsFreed();

    eTeam Team { get; }

    ITask GetFriendlyTask(BaseActorNode node);
    ITask GetHostileTask(BaseActorNode node);

    void HighlightTarget();
    void DeHighlightTarget();

}
using Godot;

public interface ITarget
{
    bool MovingTarget { get; }
    Vector2 GetTargetPosition();
}
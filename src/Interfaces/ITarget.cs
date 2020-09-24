using Godot;

public interface ITarget
{
    bool MovingTarget { get; set; }
    Vector2 GetTargetPosition();
}
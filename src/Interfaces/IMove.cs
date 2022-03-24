using Godot;

public interface IMove
{
    Vector2 Position { get; set; }
    Vector2 GlobalPosition { get; set; }
    Vector2 destination { get; set; }
    Vector2 speed { get; }

    KinematicBody2D Model { get; }
    void HandleMove(float delta);
    KinematicCollision2D MoveAndCollide(Vector2 d, bool i = true, bool j = true, bool k = false);
    bool CanMove();
    void HandleCollision(KinematicCollision2D collision);

    Vector2 ToLocal(Vector2 target);
}
using Godot;

public interface IMove
{
    Vector2 Position { get; set; }
    Vector2 destination { get; set; }
    Vector2 speed { get; }
    void HandleMove(float delta);
    KinematicCollision2D MoveAndCollide(Vector2 d, bool i = true, bool j = true, bool k = false);
    bool CanMove();
}
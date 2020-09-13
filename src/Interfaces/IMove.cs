using Godot;

public interface IMove
{
    Vector2 Position { get; set; }
    Vector2 destination { get; set; }
    Vector2 speed { get; }
    void HandleMove();
}
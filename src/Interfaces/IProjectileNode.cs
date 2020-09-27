using Godot;

public interface IProjectileNode
{

    Vector2 range { get; }
    Vector2 direction { get; }
    Vector2 speed { get; set; }
    Sprite sprite { get; }
    ICaster caster { get; }
    Runtime runtime { get; }

    Vector2 Position { get; set; }
    RayCast2D raycast { get; }

    Area2D effectRadius { get; }
    void ExecQueueFree();
}

using Godot;

public interface ICanAttack
{
    float Range { get; }

    int CurrentDamage { get; }

    eDamageType damageType { get; }

    RayCast2D RayCast { get; }

    Vector2 Position { get; }

    Vector2 ToLocal(Vector2 global);
    Vector2 GlobalPosition { get; }

    BaseActorState state { get; }
}
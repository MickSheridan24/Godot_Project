using Godot;

public interface ICanAttack
{
    float Range { get; }

    int CurrentDamage { get; }

    eDamageType damageType { get; }

    Vector2 Position { get; }

    BaseActorState state { get; }
}
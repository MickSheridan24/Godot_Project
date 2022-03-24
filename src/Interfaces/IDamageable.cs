
using Godot;

public interface IDamageable : ITarget
{


    void Damage(int power, eDamageType type);

    Vector2 Position { get; }
    Vector2 GlobalPosition { get; set; }

}

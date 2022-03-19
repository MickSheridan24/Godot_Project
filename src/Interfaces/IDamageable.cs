
using Godot;

public interface IDamageable
{


    void Damage(int power, eDamageType type);

    Vector2 Position { get; }

    bool IsFreed();
}

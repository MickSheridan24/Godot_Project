using Godot;
public interface IConductElectricity
{
    Vector2 GetTargetPosition();

    Vector2 Position { get; }
    Vector2 GlobalPosition { get; }

    void AddJoltedEffect(int dur);

    bool HasStatusEffect(eStatusEffect effect);

    Vector2 ToLocal(Vector2 pos);
}

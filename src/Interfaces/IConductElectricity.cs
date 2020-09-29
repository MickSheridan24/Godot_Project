using Godot;
public interface IConductElectricity
{
    Vector2 GetTargetPosition();

    void AddJoltedEffect(int dur);

    bool HasStatusEffect(eStatusEffect effect);
}

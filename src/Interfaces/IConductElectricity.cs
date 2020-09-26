using Godot;
public interface IConductElectricity
{
    Vector2 GetTargetPosition();

    void EnterDamageState(int amount);

    void AddStatusEffect(IStatusEffect status);
}

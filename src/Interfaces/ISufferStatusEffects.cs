
public interface ISufferStatusEffects
{

    BaseActorState state { get; }

    void TakeDamage(int amount);
    void BecomeIntangible();
    void EndIntangible();
    void RemoveEffect(eStatusEffect eff);
}

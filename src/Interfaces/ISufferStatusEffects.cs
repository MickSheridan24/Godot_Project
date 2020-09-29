
public interface ISufferStatusEffects
{


    void TakeDamage(int amount);
    void BecomeIntangible();
    void EndIntangible();
    void RemoveEffect(eStatusEffect eff);
}

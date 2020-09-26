
public interface IStatusEffect
{
    void Reduce();
    void Enact(ISufferStatusEffects target);

    int duration { get; set; }
    eStatusEffect type { get; set; }

    void Increase(int duration);
}

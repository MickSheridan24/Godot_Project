
public interface IStatusEffect
{
    bool Enact(ISufferStatusEffects target);
    void Reverse(ISufferStatusEffects target);
    int maxDuration { get; set; }
    eStatusEffect type { get; set; }
}

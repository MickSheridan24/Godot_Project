
public interface IStatusEffect
{
    void Enact(ISufferStatusEffects target);
    void Reverse(ISufferStatusEffects target);
    int maxDuration { get; set; }
    eStatusEffect type { get; set; }
}

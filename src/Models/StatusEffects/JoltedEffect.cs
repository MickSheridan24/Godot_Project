
public class JoltedEffect : StatusEffect, IStatusEffect
{

    private int tick = 60;
    public JoltedEffect() : base(eStatusEffect.JOLTED)
    {
        maxDuration = 10;
    }
    public void Enact(ISufferStatusEffects target)
    {
        tick--;
        if (tick <= 0)
        {
            target.TakeDamage(5);
        }
        tick = 60;
    }

    public void Reverse(ISufferStatusEffects target)
    {
        return;
    }
}

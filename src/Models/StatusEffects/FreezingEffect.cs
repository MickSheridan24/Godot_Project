
public class FreezingEffect : StatusEffect, IStatusEffect
{

    public int tick = 60;

    private int max = 600;

    public FreezingEffect() : base(eStatusEffect.FREEZING)
    {
        maxDuration = 10;
    }

    public bool Enact(ISufferStatusEffects target)
    {
        tick--;
        max--;
        if (tick <= 0)
        {
            target.TakeDamage(5);
            target.state.speed.AddEffect("freeze", (int i) => i / 3, true);
            tick = 60;

        }
        return max > 0;

    }

    public void Reverse(ISufferStatusEffects target)
    {
        target.state.speed.RemoveEffect("freeze", true);
    }
}


public class FreezingEffect : StatusEffect, IStatusEffect
{

    public int tick = 60;

    public FreezingEffect() : base(eStatusEffect.FREEZING)
    {
        maxDuration = 10;
    }

    public void Enact(ISufferStatusEffects target)
    {
        tick--;
        if (tick <= 0)
        {
            target.TakeDamage(5);
            target.state.speed.AddEffect("freeze", (int i) => i / 3, true);
        }
        tick = 60;
    }

    public void Reverse(ISufferStatusEffects target)
    {
        return;
    }
}


public class JoltedEffect : StatusEffect, IStatusEffect
{

    public JoltedEffect(int duration) : base(eStatusEffect.JOLTED, duration)
    {

    }
    public void Enact(ISufferStatusEffects target)
    {
        target.TakeDamage(5);
    }

    public void Increase(int dur)
    {
        duration += dur;

        if (duration > 10)
        {
            duration = 10;
        }
    }
}

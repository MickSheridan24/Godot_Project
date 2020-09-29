
public class IntangibleEffect : StatusEffect, IStatusEffect
{
    private bool enacted;
    public IntangibleEffect() : base(eStatusEffect.INTANGIBLE)
    {
        maxDuration = 10;
        enacted = false;
    }
    public void Enact(ISufferStatusEffects target)
    {
        if (!enacted)
        {
            target.BecomeIntangible();
            enacted = true;
        }
    }

    public void Reverse(ISufferStatusEffects target)
    {
        target.EndIntangible();
    }
}

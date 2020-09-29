
using System;

public class StatusEffect
{

    public eStatusEffect type { get; set; }

    public int maxDuration { get; set; }
    bool enacted;

    public StatusEffect(eStatusEffect type)
    {
        this.type = type;
        enacted = false;
    }

    internal static IStatusEffect Create(eStatusEffect s)
    {
        switch (s)
        {
            case eStatusEffect.INTANGIBLE:
                return new IntangibleEffect();
            case eStatusEffect.JOLTED:
                return new JoltedEffect();
            default:
                return null;
        }
    }
}



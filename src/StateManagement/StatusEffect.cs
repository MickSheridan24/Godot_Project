
public class StatusEffect
{

    public eStatusEffect type { get; set; }

    public int duration { get; set; }

    public StatusEffect(eStatusEffect type, int duration)
    {
        this.type = type;
        this.duration = duration;
    }
    public void Reduce()
    {
        duration -= 1;
    }

}

public enum eStatusEffect
{
    JOLTED,
    BURNING
}

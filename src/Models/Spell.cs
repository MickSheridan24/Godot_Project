public class Spell
{
    public string name { get; set; }
    public string text { get; set; }
    public eSpell type { get; set; }
}

public enum eSpell
{
    FIREBALL,
    LIGHTNING
}
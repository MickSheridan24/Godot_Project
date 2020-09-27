public interface ISpell
{
    string name { get; set; }
    string text { get; set; }
    eSpell type { get; set; }
    void Cast(Wizard caster);

}

public class FarmSpell : Spell, ISpell
{
    public FarmSpell()
    {
        name = "Grow Farm";
        type = eSpell.CREATE_FARM;
        text = "CORNSIDER THIS";
    }

    public void Cast(Wizard caster)
    {
        var target = caster.runtime.RightTarget;

        caster.runtime.World.CreateStructure(target.GetTargetPosition(), new Farm());
    }
}

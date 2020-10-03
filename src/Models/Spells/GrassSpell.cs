
public class GrassSpell : Spell, ISpell
{

    public GrassSpell()
    {
        name = "Create Grass";
        type = eSpell.CREATE_GRASS;
        text = "ITS GOT WHAT PLANTS CRAVE";
    }
    public void Cast(Wizard caster)
    {
        var target = caster.runtime.RightTarget;

        caster.runtime.World.CreateGrassPatch(target.GetTargetPosition());
    }
}

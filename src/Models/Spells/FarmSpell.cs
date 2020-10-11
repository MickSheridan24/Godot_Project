
using System.Collections.Generic;

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

    public List<UIEffect> GetUIHints(Wizard caster)
    {
        var target = caster.runtime.RightTarget;

        var Circle = (CircleHighlight)snCircleHighlight.Instance();
        Circle.color = new UITheme().cRed;
        Circle.radius = 10;

        return new List<UIEffect>(){
            Circle
        };
    }
}

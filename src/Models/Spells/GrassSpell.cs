
using System.Collections.Generic;

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
    public List<UIEffect> GetUIHints(Wizard caster)
    {
        var target = caster.runtime.RightTarget;

        var Circle = (CircleHighlight)snCircleHighlight.Instance();
        Circle.color = new UITheme().cRed;
        Circle.radius = 10;
        Circle.origin = target;
        Circle.ZAsRelative = true;
        Circle.ZIndex = 3;

        return new List<UIEffect>(){
            Circle
        };
    }
}

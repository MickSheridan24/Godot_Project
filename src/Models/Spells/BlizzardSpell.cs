
using System.Collections.Generic;
using Godot;

public class BlizzardSpell : Spell, ISpell
{
    public static int effectRadius = 600;


    public BlizzardSpell()
    {
        name = "Blizzard";
        type = eSpell.BLIZZARD;
        text = "EVERYBODY COOL DOWN";
    }
    public void Cast(Wizard caster)
    {
        var target = caster.runtime.RightTarget;
        var pos = caster.ToLocal(caster.Position);

        var targetPos = target?.GetTargetPosition() ?? caster.GlobalPosition + new Vector2(1, 1);
        var projectileDetails = new BlizzardProjectile()
        {
            direction = pos.GetDirectionTo(caster.ToLocal(targetPos)),
            start = new Vector2(targetPos.x, targetPos.y)
        };
        caster.CreateProjectile(projectileDetails);
    }

    public List<UIEffect> GetUIHints(Wizard caster)
    {
        var target = caster.runtime.RightTarget;

        var Circle = (CircleHighlight)snCircleHighlight.Instance();
        Circle.color = new UITheme().cBlue;
        Circle.radius = effectRadius;
        Circle.origin = target;

        return new List<UIEffect>(){
            Circle
        };
    }
}
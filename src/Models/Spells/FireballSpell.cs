
using System.Collections.Generic;
using Godot;

public class FireballSpell : Spell, ISpell
{
    public static int effectRadius = 200;


    public FireballSpell()
    {
        name = "Cast Fireball";
        type = eSpell.FIREBALL;
        text = "FIREBALL";
    }
    public void Cast(Wizard caster)
    {
        var target = caster.runtime.RightTarget;
        var pos = caster.ToLocal(caster.Position);

        var targetPos = target?.GetTargetPosition() ?? caster.GlobalPosition + new Vector2(1, 1);
        var projectileDetails = new FireballProjectile()
        {
            direction = pos.GetDirectionTo(caster.ToLocal(targetPos)),
            start = pos
        };
        caster.CreateProjectile(projectileDetails);
    }

    public List<UIEffect> GetUIHints(Wizard caster)
    {
        var target = caster.runtime.RightTarget;

        var Circle = (CircleHighlight)snCircleHighlight.Instance();
        Circle.color = new UITheme().cRed;
        Circle.radius = effectRadius;
        Circle.origin = target;

        return new List<UIEffect>(){
            Circle
        };
    }
}
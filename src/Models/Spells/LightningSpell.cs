using System.Collections.Generic;
using Godot;

public class LightningSpell : Spell, ISpell
{
    public LightningSpell()
    {
        name = "Cast Lightning";
        type = eSpell.LIGHTNING;
        text = "LIGHTNING BOLT";
    }
    public void Cast(Wizard caster)
    {
        var target = caster.runtime.RightTarget;
        var targetPos = target?.GetTargetPosition() ?? caster.spritePosition + new Vector2(1, 1);
        var projectileDetails = new LightningProjectile(20)
        {
            direction = caster.Position.GetDirectionTo(target.GetTargetPosition()),
            start = caster.spritePosition,
            damage = 100
        };
        caster.CreateProjectile(projectileDetails);
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
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

        var line = (LineHighlight)snLineHighlight.Instance();
        line.color = new UITheme().cRed;
        line.target = caster.runtime.RightTarget;
        line.origin = caster;
        line.length = new Vector2(50, 50);

        return new List<UIEffect>(){
            line
        };
    }
}
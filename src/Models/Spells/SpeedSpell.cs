
using System.Collections.Generic;

public class SpeedSpell : Spell, ISpell
{
    public SpeedSpell()
    {
        name = "Gain Speed";
        type = eSpell.SPEED;
        text = "GOTTA GO FAST";
    }
    public void Cast(Wizard caster)
    {
        var stats = caster.state as IHaveStats;

        if (stats.speed.current == stats.speed.standard)
        {
            stats.speed.current *= 2;
            stats.tickHandler.AddOrder(new EndBuffOrder(stats.speed), 10);
        }
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

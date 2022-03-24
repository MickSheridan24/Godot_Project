
using System;
using System.Collections.Generic;
using Godot;

public class WallSpell : Spell, ISpell
{

    private Vector2 V1;
    private Vector2 V2;

    private Vector2 maxLength = new Vector2(1000, 1000);
    private Wizard caster;

    public WallSpell()
    {
        name = "Build Wall";
        type = eSpell.EARTH_WALL;
        text = "WE WILL BUILD THAT WALL";
    }
    public void Cast(Wizard caster)
    {
        var rightPosition = caster.runtime.RightTarget;
        var leftPosition = caster.runtime.LeftTarget;

        this.caster = caster;

        SetVectors(rightPosition, leftPosition);
        if (V2 == null)
        {
            caster.runtime.World.CreateEarthWall(V1);
        }
        else
        {
            caster.runtime.World.CreateEarthWall(V1, V2);
        }
    }

    private void SetVectors(ITarget rightPosition, ITarget leftPosition)
    {
        var right = rightPosition != null;
        var left = leftPosition != null;

        if (right && left)
        {
            V1 = rightPosition.GetTargetPosition();
            V2 = leftPosition.GetTargetPosition().ClosestInRange(V1, maxLength);
        }
        else if (left)
        {
            V1 = leftPosition.GetTargetPosition();
        }
        else if (right)
        {
            V1 = rightPosition.GetTargetPosition();
        }
        else
        {
            V1 = caster.GlobalPosition;
        }
    }

    public List<UIEffect> GetUIHints(Wizard caster)
    {
        var rightPosition = caster.runtime.RightTarget;
        var leftPosition = caster.runtime.LeftTarget;

        SetVectors(rightPosition, leftPosition);

        var effects = new List<UIEffect>();

        if (V1 != null)
        {
            var rightCircle = (CircleHighlight)snCircleHighlight.Instance();
            rightCircle.color = new UITheme().cBlue;
            rightCircle.radius = 40;
            rightCircle.origin = new VectorTarget(V1);
            effects.Add(rightCircle);
        }
        if (V2 != null)
        {
            var leftCircle = (CircleHighlight)snCircleHighlight.Instance();
            leftCircle.color = new UITheme().cBlue;
            leftCircle.radius = 40;
            leftCircle.origin = new VectorTarget(V2);
            effects.Add(leftCircle);
        }
        if (V1 != null && V2 != null)
        {
            var line = (LineHighlight)snLineHighlight.Instance();
            line.origin = new VectorTarget(V1);
            line.target = new VectorTarget(V2);
            line.length = (V2 - V1).Abs();
            line.color = new UITheme().cBlue;
            effects.Add(line);
        }

        return effects;
    }
}

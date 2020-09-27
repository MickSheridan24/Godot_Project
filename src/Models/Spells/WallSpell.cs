
using System;
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
            V1 = caster.Position;
        }
    }
}

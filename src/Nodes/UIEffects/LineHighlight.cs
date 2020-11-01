
using System;
using Godot;

public class LineHighlight : UIEffect
{
    public Vector2 length { get; set; }
    public Color color { get; set; }

    public ITarget target { get; set; }


    public Vector2 dir { get; private set; }

    public override void _Process(float d)
    {
        if (GetPointDir() != dir)
        {
            dir = GetPointDir();
            Update();
        }
        if (Position != origin.GetTargetPosition())
        {
            Update();
        }
    }

    private Vector2 GetPointDir()
    {
        return origin.GetTargetPosition().GetDirectionTo(target.GetTargetPosition());
    }

    public override void _Draw()
    {
        Position = origin.GetTargetPosition();
        ZIndex = 1;
        DrawLine(Vector2.Zero, dir * length, color, 4);
    }
}

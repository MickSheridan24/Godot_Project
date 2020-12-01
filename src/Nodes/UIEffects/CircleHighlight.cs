using Godot;
using System;

public class CircleHighlight : UIEffect
{

    public int radius { get; set; }
    public Color color { get; set; }


    public override void _Process(float d)
    {
        if (origin != null)
        {
            if (Position != origin.GetTargetPosition())
            {
                Position = origin.GetTargetPosition();
                Update();
            }
        }
    }

    public override void _Draw()
    {
        if (origin != null)
        {
            Position = origin.GetTargetPosition();
            ZIndex = 1;
            DrawCircle(Vector2.Zero, radius, color);
        }
    }


}

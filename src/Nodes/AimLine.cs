using Godot;
using System;

public class AimLine : Node2D
{
    public UITheme theme => new UITheme();
    public Vector2 dest { get; set; }


    public override void _Draw()
    {
        var dir = GlobalPosition.GetDirectionTo(dest);
        DrawLine(Vector2.Zero, dir * new Vector2(20, 20), theme.cAccent, 4);
    }
}

using Godot;
using System;

public class Cursor : Node2D
{
    public UITheme theme => new UITheme();

    public override void _Process(float delta)
    {
        var pos = GetGlobalMousePosition();
        if (pos != Vector2.Zero && pos != Position)
        {
            Position = pos;
            Update();
        }
    }

    public override void _Draw()
    {
        DrawCircle(GetGlobalMousePosition(), 15, theme.cPrimary);
    }

}

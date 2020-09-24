using Godot;
using System;

public class Highlight : Node2D
{
    public UITheme theme => new UITheme();
    public Vector2 position { get; set; }

    public override void _Draw()
    {
        var r = 20;
        var nr = -20;
        DrawLine(position + new Vector2(nr, nr), position + new Vector2(nr, r), theme.cAccent, 3);
        DrawLine(position + new Vector2(nr, nr), position + new Vector2(r, nr), theme.cAccent, 3);
        DrawLine(position + new Vector2(nr, r), position + new Vector2(r, r), theme.cAccent, 3);
        DrawLine(position + new Vector2(r, nr), position + new Vector2(r, r), theme.cAccent, 3);
    }
}
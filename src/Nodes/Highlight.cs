using Godot;
using System;

public class Highlight : Node2D
{
    public UITheme theme => new UITheme();
    public Vector2 position { get; set; }

    public Color color { get; set; }

    public override void _Draw()
    {
        var r = 20;
        var nr = -20;
        DrawLine(position + new Vector2(nr, nr), position + new Vector2(nr, r), color, 3);
        DrawLine(position + new Vector2(nr, nr), position + new Vector2(r, nr), color, 3);
        DrawLine(position + new Vector2(nr, r), position + new Vector2(r, r), color, 3);
        DrawLine(position + new Vector2(r, nr), position + new Vector2(r, r), color, 3);
    }
}
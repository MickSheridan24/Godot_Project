using Godot;
using System;

public class Highlight : Node2D
{
    public UITheme theme => new UITheme();
    public Vector2 position { get; set; }
    public Vector2 size { get; set; }
    public Color color { get; set; }

    public override void _Ready()
    {
        this.size = GetParent<IHaveSize>().size;
    }

    public override void _Draw()
    {
        var lowY = -1 * size.y / 2;
        var highY = size.y / 2;
        var lowX = -1 * size.x / 2;
        var highX = size.x / 2;

        DrawLine(position + new Vector2(lowX, lowY), position + new Vector2(lowX, highY), color, 3);
        DrawLine(position + new Vector2(lowX, lowY), position + new Vector2(highX, lowY), color, 3);
        DrawLine(position + new Vector2(lowX, highY), position + new Vector2(highX, highY), color, 3);
        DrawLine(position + new Vector2(highX, lowY), position + new Vector2(highX, highY), color, 3);
    }
}
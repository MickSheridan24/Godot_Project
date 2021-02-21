using Godot;
using System;

public class DragSelect : Node2D
{
    public bool isDragging { get; set; }
    public Vector2 position { get; set; }
    public Vector2 size { get; set; }
    public Color color { get; set; }
    public override void _Ready()
    {
        this.size = Vector2.Zero;
        this.position = Vector2.Zero;
        this.color = new Color("#1983bd");
        isDragging = false;
    }

    public override void _Process(float delta)
    {
        if (isDragging)
        {
            size = GetGlobalMousePosition() - position;
            Visible = true;
            Update();
        }
        else
        {
            this.size = Vector2.Zero;
            Visible = false;
            Update();
        }
    }


    public override void _Draw()
    {
        var lowY = 0;
        var highY = size.y;
        var lowX = 0;
        var highX = size.x;

        DrawLine(position + new Vector2(lowX, lowY), position + new Vector2(lowX, highY), color, 3);
        DrawLine(position + new Vector2(lowX, lowY), position + new Vector2(highX, lowY), color, 3);
        DrawLine(position + new Vector2(lowX, highY), position + new Vector2(highX, highY), color, 3);
        DrawLine(position + new Vector2(highX, lowY), position + new Vector2(highX, highY), color, 3);
    }

    public void BeginDrag()
    {
        position = GetGlobalMousePosition();
        isDragging = true;
    }

    public void EndDrag()
    {
        position = Vector2.Zero;
        isDragging = false;
    }
}

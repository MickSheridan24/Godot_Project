using Godot;

public class UIHighlight : Control
{
    public UITheme theme => new UITheme();
    public Vector2 position => RectPosition;
    public Vector2 size => GetParent<Control>().RectSize;
    public Color color { get; set; }

    public override void _Ready()
    {
        color = new Color("#ffffff");
        Visible = false;

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
}

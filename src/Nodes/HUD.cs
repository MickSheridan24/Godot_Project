using Godot;
using System;

public class HUD : Node2D, IHaveRuntime
{
    public Camera2D camera => GetNode<Camera2D>("Camera2D");
    public Runtime runtime => GetParent<IHaveRuntime>().runtime;
    private Vector2 panSpeed => new Vector2(10, 10);
    private UITheme theme => new UITheme();
    private Rect2 bounds;

    public override void _Ready()
    {
        bounds = GetBounds();
    }
    public override void _Process(float d)
    {
        var c = bounds.Position + bounds.Size / 2;

        bounds = GetBounds();
        var pos = GetGlobalMousePosition();
        var center = GetViewportRect().Size / 2 + Position;

        if (!pos.InBounds(bounds.Position, bounds.Position + bounds.Size))
        {

            var directi = c.DirectionTo(pos);

            GlobalPosition += (directi * panSpeed);

            bounds = GetBounds();
        }
    }

    private Rect2 GetBounds()
    {
        var origin = GlobalPosition - (GetViewportRect().Size * camera.Zoom / 2) + new Vector2(50, 50) * camera.Zoom;
        return new Rect2(origin, GetViewportRect().Size * camera.Zoom - new Vector2(100, 100) * camera.Zoom);
    }
    // public override void _Draw()
    // {
    //     bounds = GetBounds();
    //     var point1 = bounds.Position;
    //     var point2 = bounds.Position + bounds.Size * new Vector2(0, 1);
    //     var point3 = bounds.Position + bounds.Size * new Vector2(1, 0);
    //     var point4 = bounds.Position + bounds.Size;

    //     DrawLine(point1, point2, theme.cRed);
    //     DrawLine(point1, point3, theme.cRed);
    //     DrawLine(point2, point4, theme.cRed);
    //     DrawLine(point3, point4, theme.cRed);

    // }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton)
        {
            InputEventMouseButton emb = (InputEventMouseButton)@event;
            if (emb.IsPressed())
            {
                if (emb.ButtonIndex == (int)ButtonList.WheelUp && camera.Zoom <= new Vector2(4, 4))
                {
                    camera.Zoom += new Vector2(0.1f, 0.1f);
                }
                if (emb.ButtonIndex == (int)ButtonList.WheelDown && camera.Zoom >= new Vector2(1, 1))
                {
                    camera.Zoom -= new Vector2(0.1f, 0.1f);
                }

            }
        }
    }

}

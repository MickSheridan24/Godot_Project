using Godot;
using System;

public class MenuButton : ColorRect
{
    private bool mouseIn;
    public string Hotkey { get; set; }
    public Func<bool> Action { get; internal set; }

    public override void _Ready()
    {
        mouseIn = false;
        Set("color", new UITheme().cAccent);
    }

    public override void _Input(InputEvent @event)
    {

        if (Visible && Action != null && GetGlobalMousePosition().InBounds(GetRect().Position, GetRect().Position + GetRect().Size))
        {
            GD.Print("CLICK BUTTON");
            Action();

            GetTree().SetInputAsHandled();
        }
    }

}


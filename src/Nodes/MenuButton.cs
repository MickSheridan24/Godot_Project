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
        if (mouseIn)
        {
            GD.Print("CLICK BUTTON");
            Action();
        }
    }
    public void _on_MenuButton_mouse_entered()
    {
        mouseIn = true;
    }

    public void _on_MenuButton_mouse_exited()
    {
        mouseIn = false;
    }
}


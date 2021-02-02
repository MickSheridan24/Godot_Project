using Godot;
using System;

public class MenuButton : Button
{
    private bool mouseIn;
    public string Hotkey { get; set; }
    public Func<bool> Action { get; internal set; }


    public int flashFrames;

    public override void _Ready()
    {
        mouseIn = false;

        flashFrames = 0;
        Connect("pressed", this, "_button_pressed");

    }

    public override void _Input(InputEvent @event)
    {
        if (Visible && @event is InputEventKey && @event.AsText() == Hotkey)
        {
            Click();
        }
    }
    public override void _PhysicsProcess(float delta)
    {
        Text = Hotkey;
        //    flashFrames--;
        if (flashFrames <= 0)
        {
            Pressed = false;
            flashFrames = 0;
            (Get("custom_styles/normal") as StyleBoxFlat).Set("bg_color", new Color("#2060c6"));
        }
    }

    public void Click()
    {

        GD.Print("CLICK BUTTON");
        if (Action())
        {
            FlashColor(new UITheme().cGreen);
        }
        else
        {
            FlashColor(new UITheme().cRed);
        }


        GetTree().SetInputAsHandled();

    }

    private void FlashColor(Color color)
    {
        Pressed = true;
        flashFrames = 200;
        (Get("custom_styles/normal") as StyleBoxFlat).Set("bg_color", color);
    }

    private void _button_pressed()
    {
        GD.Print("CLICK");
        Click();
    }

}







using Godot;
using System;

public class SpellHandler : Control
{

    public Runtime runtime => GetParent<IHaveRuntime>().runtime;
    public ColorRect panel => GetNode<ColorRect>("ColorRect");
    public Label textPanel => GetNode<Label>("SpellText");
    public UITheme theme;

    public override void _Ready()
    {
        theme = new UITheme();
        panel.Color = theme.cPrimary;
        textPanel.Set("custom_colors/font_color", theme.cAccent);
    }

    public override void _Process(float f)
    {
        if (runtime.IsCasting != Visible)
        {
            Visible = runtime.IsCasting;
        }
    }

    public override void _Input(InputEvent e)
    {
        if (runtime.IsCasting)
        {
            var key = e.GetAlphaOrSpaceJustPressed();
            if (key != -1)
            {
                GD.Print(e.AsText());
                textPanel.Text += key == (int)KeyList.Space ? " " : e.AsText();
            }
        }

    }
}
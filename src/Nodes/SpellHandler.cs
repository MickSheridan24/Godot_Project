using Godot;
using System;

public class SpellHandler : Control
{

    public Runtime runtime => GetParent<IHaveRuntime>().runtime;
    public ColorRect panel => GetNode<ColorRect>("ColorRect");
    public Label spellName => GetNode<Label>("SpellName");
    public Label entireText => GetNode<Label>("SpellText/EntireText");
    public Label completedText => GetNode<Label>("SpellText/EntireText/CompletedText");
    public ISpell currentSpell => runtime.GetCurrentSpell();
    public UITheme theme;

    public override void _Ready()
    {
        theme = new UITheme();
        panel.Color = theme.cPrimary;

        entireText.Set("custom_colors/font_color", theme.cAccent);
        completedText.Set("custom_colors/font_color", theme.cBlue);
    }

    public override void _Process(float f)
    {
        Visible = runtime.IsCasting;
        if (Visible)
        {
            entireText.Text = currentSpell.text;
            spellName.Text = currentSpell.name;
        }
        else
        {
            completedText.Text = "";
        }
    }

    public override void _UnhandledInput(InputEvent e)
    {
        if (runtime.IsCasting)
        {
            var key = e.GetAlphaOrSpaceJustPressed();
            if (key != -1)
            {
                HandleKeyPress(GetKey(key, e));
            }
        }
    }

    private string GetKey(int keyCode, InputEvent e)
    {
        return keyCode == (int)KeyList.Space ? " " : e.AsText();
    }
    private void HandleKeyPress(string key)
    {
        var result = runtime.UpdateCast(key);
        if (result.success)
        {
            completedText.Text = result.text;
        }
        if (result.complete)
        {
            completedText.Text = "";
        }
    }
}
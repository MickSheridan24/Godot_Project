using Godot;
using System;

public class UIMenu : ColorRect, IHaveRuntime
{

    public Runtime runtime => GetParent<IHaveRuntime>().runtime;

    public Control entityMenu => GetNode<Control>("EntityMenu");
    public Control spellText => GetNode<Control>("SpellText");
    public Label entireText => GetNode<Label>("SpellText/EntireText");
    public Label completedText => GetNode<Label>("SpellText/EntireText/CompletedText");
    public Label spellName => GetNode<Label>("SpellText/SpellName");

    public ISpell currentSpell => runtime.GetCurrentSpell();
    public UITheme theme;
    public PartialMenu partial => GetNode<PartialMenu>("PartialMenu");

    public override void _Ready()
    {
        theme = new UITheme();
        Color = theme.cPrimary;

        entireText.Set("custom_colors/font_color", theme.cAccent);
        completedText.Set("custom_colors/font_color", theme.cBlue);
    }

    public override void _Process(float f)
    {
        Visible = runtime.IsCasting;
        if (Visible && runtime.IsCasting)
        {
            if (runtime.WizardIsSelected())
            {
                spellText.Visible = true;
                entityMenu.Visible = false;
                partial.Visible = false;

                entireText.Text = currentSpell.text;
                spellName.Text = currentSpell.name;
            }
            else if (runtime.currentSelection != null)
            {
                spellText.Visible = false;
                entityMenu.Visible = true;
                partial.Visible = true;
                ConfigureEntityMenu();
            }

        }
        else
        {
            completedText.Text = "";
        }
    }


    private void ConfigureEntityMenu()
    {
        entityMenu.GetNode<Label>("Nametag").Text = runtime.currentSelection.EntityName;
        entityMenu.GetNode<Label>("Description").Text = runtime.currentSelection.Description;

        if (partial.state != runtime.currentSelection.GetMenuState())
        {
            partial.Config(runtime.currentSelection.GetMenuState());
            partial.ResetButtons();
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (runtime.IsCasting && runtime.WizardIsSelected() && @event is InputEventKey)
        {
            var key = @event.GetAlphaOrSpaceJustPressed();
            if (key != -1)
            {
                HandleKeyPress(GetKey(key, @event));
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



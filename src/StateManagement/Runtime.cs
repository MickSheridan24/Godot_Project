using System;
using Godot;

public class Runtime
{
    public ISelectable currentSelection { get; set; }
    public WizardState WizardState { get; set; }
    public Wizard wizardNode;
    public UIState UIState { get; }
    public UI UINode;
    public Vector2 hoveredCell;
    public CastManager castManager { get; set; }

    public bool IsCasting;

    public Runtime()
    {
        this.WizardState = new WizardState();
        this.UIState = new UIState();
    }


    public void SelectWizard()
    {
        currentSelection = wizardNode;
    }

    public void RegisterWizard(Wizard wizard)
    {
        wizardNode = wizard;
    }

    public void RegisterUI(UI ui)
    {
        UINode = ui;
    }

    public void InitCast()
    {
        castManager = new CastManager(WizardState);
    }
    public void ToggleCasting()
    {
        ToggleCasting(!IsCasting);
    }
    public void ToggleCasting(bool value)
    {
        IsCasting = value;
        if (IsCasting)
        {
            InitCast();
        }
        if (!IsCasting)
        {
            castManager = null;
        }
    }

    public Spell GetCurrentSpell()
    {
        return castManager?.currentSpell ?? null;
    }
    public SpellCastResult UpdateCast(string key)
    {
        var result = castManager.UpdateCast(key);
        if (result.complete)
        {
            GD.Print(result.Spell.name + " Cast");
            ToggleCasting(false);
        }
        return result;
    }
}
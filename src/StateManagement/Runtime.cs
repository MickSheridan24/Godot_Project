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

    public void ToggleCasting()
    {
        IsCasting = !IsCasting;
        GD.Print("TOGGLE ", IsCasting);
    }
}
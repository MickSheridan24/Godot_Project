using System;

public class Runtime
{
    public ISelectable currentSelection { get; set; }
    public WizardState WizardState { get; set; }
    public Runtime()
    {
        this.WizardState = new WizardState();
    }
}
using Godot;
using System;

public class Debug : Control
{
    public Label WizardPos => GetNode<Label>("Panel/WizardPosition");
    public Label ProjStats => GetNode<Label>("Panel/ProjectileStats");
    public Label MouseStats => GetNode<Label>("Panel/MousePosition");
    public Runtime runtime => GetParent<IHaveRuntime>().runtime;
    //public SimpleProjectile projectile { get; set; }
    public override void _Process(float d)
    {
        WizardPos.Text = "WIZ: " + runtime.wizardNode.Position.ToString();
        ProjStats.Text = runtime.currentTarget?.GetTargetPosition().ToString() ?? "";

        MouseStats.Text = runtime.MousePosition.ToString();
    }
}
using Godot;
using System;

public class Debug : Control
{
    public Label D1 => GetNode<Label>("Panel/D1");
    public Label D2 => GetNode<Label>("Panel/D2");
    public Label D3 => GetNode<Label>("Panel/D3");
    public Runtime runtime => GetParent().GetParent<IHaveRuntime>().runtime;

    public DebugInfo info => runtime.Debug;
    public override void _Process(float d)
    {
        D1.Text = info.D1;
        D2.Text = info.D2;
        D3.Text = info.D3;
    }
}
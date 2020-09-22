using Godot;
using System;

public class Main : Node2D, IHaveRuntime
{
    public Runtime runtime { get; set; }
    public Wizard wizard => GetNode<Wizard>("Wizard");
    public UI UI => GetNode<UI>("UI");
    public Debug debug => GetNode<Debug>("Debug");
    public override void _Ready()
    {
        runtime = new Runtime();
        runtime.RegisterWizard(wizard);
        runtime.RegisterUI(UI);
        runtime.Debug = debug;
    }

    public override void _Process(float d)
    {
        runtime.MousePosition = GetGlobalMousePosition();
    }
    public override void _Input(InputEvent @event)
    {
        if ((@event as InputEventMouseButton)?.ButtonIndex == (int)ButtonList.Right &&
            runtime.currentSelection != null)
        {
            runtime.currentSelection.RightClick(@event as InputEventMouseButton);
        }
        else if (@event.GetKeyJustPressed() == (int)KeyList.Space && !runtime.IsCasting)
        {
            runtime.SelectWizard();
        }
        else if (@event.GetKeyJustPressed() == (int)KeyList.Shift)
        {
            runtime.ToggleCasting();
        }
    }

}

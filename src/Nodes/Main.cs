using Godot;
using System;

public class Main : Node2D, IHaveRuntime
{
    public Runtime runtime { get; set; }
    public Wizard wizard => GetNode<Wizard>("Wizard");
    public World World => GetNode<World>("World");
    public StructureNode Cottage => GetNode<StructureNode>("Structure");

    public override void _Ready()
    {
        runtime = new Runtime();
        runtime.RegisterWizard(wizard);
        runtime.RegisterWorld(World);

        Cottage.state = new Cottage();
        Cottage.Configure();


        // GetNode<Enemy>("Enemy").Name = "Beevis";
        // GetNode<Enemy>("Enemy2").Name = "Goonie";
        // GetNode<Enemy>("Enemy3").Name = "Grubley";
    }

    public override void _Process(float d)
    {
        runtime.MousePosition = GetGlobalMousePosition();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.RightClickJustPressed())
        {
            runtime.SetRightTarget(new VectorTarget(GetGlobalMousePosition()));
            if (runtime.currentSelection != null)
            {
                runtime.currentSelection.RightClick(@event as InputEventMouseButton);
            }
        }

        if (@event.LeftClickJustPressed())
        {
            runtime.SetLeftTarget(new VectorTarget(GetGlobalMousePosition()));
        }

        if (@event.GetKeyJustPressed() == (int)KeyList.Space && !runtime.IsCasting)
        {
            runtime.SelectWizard();
        }
        else if (@event.GetKeyJustPressed() == (int)KeyList.Shift)
        {
            runtime.ToggleCasting();
        }
    }

}

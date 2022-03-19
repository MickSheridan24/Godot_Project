using Godot;
using System;

public class Main : Node2D, IHaveRuntime
{
    public Runtime runtime { get; set; }
    public Wizard wizard => GetNode<Wizard>("Wizard");
    public World World => GetNode<World>("World");

    public Enemy enemy => GetNode<Enemy>("Enemy");
    public StructureNode Cottage => GetNode<StructureNode>("Structure");



    public override void _Ready()
    {
        Input.SetMouseMode(Input.MouseMode.Confined);
        runtime = new Runtime();
        runtime.RegisterWizard(wizard);
        runtime.RegisterWorld(World);
        runtime.RegisterEnemy(enemy);

        Cottage.state = new Cottage(runtime.playerState);
        Cottage.Configure(eTeam.FRIENDLY);
    }

    public override void _Input(InputEvent @event)
    {

        if (runtime.inputHandler.HandleInput(@event, GetGlobalMousePosition()))
        {
            GetTree().SetInputAsHandled();
        }
    }

}

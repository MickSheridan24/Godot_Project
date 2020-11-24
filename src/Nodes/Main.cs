using Godot;
using System;

public class Main : Node2D, IHaveRuntime
{
    public Runtime runtime { get; set; }
    public Wizard wizard => GetNode<Wizard>("Wizard");
    public World World => GetNode<World>("World");
    public StructureNode Cottage => GetNode<StructureNode>("Structure");

    public InputHandler inputHandler;

    public override void _Ready()
    {
        runtime = new Runtime();
        runtime.RegisterWizard(wizard);
        runtime.RegisterWorld(World);

        Cottage.state = new Cottage();
        Cottage.Configure();

        inputHandler = new InputHandler(runtime);

    }

    public override void _Input(InputEvent @event)
    {
        inputHandler.HandleInput(@event, GetGlobalMousePosition());
    }

}

using Godot;
using System;

public class Main : Node2D, IHaveRuntime
{
    public Runtime runtime { get; set; }
    public Wizard wizard => GetNode<Wizard>("Wizard");
    public World World => GetNode<World>("World");
    public NPC npc => GetNode<NPC>("NPC");
    public StructureNode Cottage => GetNode<StructureNode>("Structure");



    public override void _Ready()
    {
        runtime = new Runtime();
        runtime.RegisterWizard(wizard);
        runtime.RegisterWorld(World);
        runtime.RegisterNPC(npc);

        Cottage.state = new Cottage();
        Cottage.Configure();
    }

    public override void _Input(InputEvent @event)
    {

        if (runtime.inputHandler.HandleInput(@event, GetGlobalMousePosition()))
        {
            GetTree().SetInputAsHandled();
        }
    }

}

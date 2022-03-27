using Godot;
using System;

public class Main : Node2D, IHaveRuntime
{
    public Runtime runtime { get; set; }
    public Wizard wizard => GetNode<Wizard>("Wizard");
    public World World => GetNode<World>("World");

    public StructureNode Cottage => GetNode<StructureNode>("Structure");

    public Timer Timer => GetNode<Timer>("Timer");



    public override void _Ready()
    {
        Input.SetMouseMode(Input.MouseMode.Confined);
        runtime = new Runtime();
        runtime.RegisterWizard(wizard);
        runtime.RegisterTower(Cottage);

        runtime.RegisterWorld(World);
        runtime.RegisterEnemySpawner(new ZombieSpawner());

        Cottage.state = new Cottage(runtime.playerState);
        Cottage.Configure(eTeam.FRIENDLY);

        Timer.Start();
    }

    public override void _Input(InputEvent @event)
    {

        if (runtime.inputHandler.HandleInput(@event, GetGlobalMousePosition()))
        {
            GetTree().SetInputAsHandled();
        }
    }

    public void OnTimerTick()
    {

        runtime.Spawn();

    }

}

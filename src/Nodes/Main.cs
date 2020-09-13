using Godot;
using System;

public class Main : Node2D, IHaveRuntime
{
    public Runtime runtime { get; set; }
    public override void _Ready()
    {
        runtime = new Runtime();
    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton &&
            (@event as InputEventMouseButton).ButtonIndex == (int)ButtonList.Right &&
            runtime.currentSelection != null)
        {
            runtime.currentSelection.RightClick(@event as InputEventMouseButton);
        }
    }

}

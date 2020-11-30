using Godot;
using System;

public class ClickableArea2D : StaticBody2D
{

    public Runtime runtime => GetParent<IHaveRuntime>().runtime;
    public ISelectable selectable => GetParent<ISelectable>();

    public override void _Input(InputEvent @event)
    {

        var position = selectable.GetSelectionArea().Position;
        var size = selectable.GetSelectionArea().Size;

        if (@event is InputEventMouseButton && GetGlobalMousePosition().InBounds(position, position + size))
        {
            if (runtime.inputHandler.HandleInput(@event, GetGlobalMousePosition(), selectable))
            {
                GetTree().SetInputAsHandled();
            }
        }

    }
}

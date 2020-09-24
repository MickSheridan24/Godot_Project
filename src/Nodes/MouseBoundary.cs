using Godot;
using System;

public class MouseBoundary : ReferenceRect
{

    public override void _Ready()
    {
        RectSize = GetViewportRect().Size / 8;
        RectPosition = GetViewportRect().Size / 10;
    }


}

using System;
using Godot;

public class SpellTarget
{
    public Vector2 vTarget { get; set; }
    public ITarget iTarget { get; set; }
    public bool isVTarget { get; set; }

    public Vector2 GetPosition()
    {
        return isVTarget ? vTarget : iTarget.GetPosition();
    }
}
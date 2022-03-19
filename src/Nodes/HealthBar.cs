using Godot;
using System;

public class HealthBar : Node2D
{
    public IHaveHealth source => GetParent<IHaveHealth>();
    public ProgressBar bar => GetNode<ProgressBar>("ProgressBar");


    public override void _Process(float f)
    {
        bar.MaxValue = source.MaxHealth;
        bar.Value = source.Health;
    }
}

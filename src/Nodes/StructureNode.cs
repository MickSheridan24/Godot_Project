using Godot;
using System;

public class StructureNode : Node2D
{
    public IStructure state { get; set; }
    public Sprite sprite => GetNode<Sprite>("Sprite");
    public void Configure()
    {
        state.ConfigureNode(this);
    }



}

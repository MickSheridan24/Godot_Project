using Godot;
using System;

public class Soldier : NPC, ICanAttack
{
    protected PackedScene snModel => (PackedScene)ResourceLoader.Load("res://scenes/Models/SoldierModel.tscn");




    public override void _Ready()
    {
        base._Ready();

        state = new SoldierState(this);
    }

    protected override void OverrideModel()
    {
        var node = (Node2D)snModel.Instance();
        node.Name = "Model";
        AddChild(node);
        base.OverrideModel();

    }
}
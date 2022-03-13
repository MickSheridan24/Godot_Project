using Godot;
using System;

public class Soldier : NPC, ICanAttack
{
    protected PackedScene snModel => (PackedScene)ResourceLoader.Load("res://scenes/Models/SoldierModel.tscn");

    protected override void OverrideModel()
    {
        var node = (Node2D)snModel.Instance();
        AddChild(node);

    }
}
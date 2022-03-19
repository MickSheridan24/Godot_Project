using Godot;
using Mavisithor_Beaconizath.src.Interfaces;
using System;

public class Laborer : NPC, ICanDoLabor
{

    protected PackedScene snModel => (PackedScene)ResourceLoader.Load("res://scenes/Models/LaborerModel.tscn");

    protected override void OverrideModel()
    {
        var node = (Node2D)snModel.Instance();
        node.Name = "Model";
        AddChild(node);
        base.OverrideModel();


    }

}

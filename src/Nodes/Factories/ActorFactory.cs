using System;
using Godot;

public class ActorFactory
{

    private static PackedScene snNPCNode => (PackedScene)ResourceLoader.Load("res://scenes/NPC.tscn");
    private static PackedScene snLaborerNode => (PackedScene)ResourceLoader.Load("res://scenes/NPCs/Laborer.tscn");
    private static PackedScene snSoldierNode => (PackedScene)ResourceLoader.Load("res://scenes/NPCs/Soldier.tscn");
    public static NPC CreateNPC(IHaveRuntime parent)
    {
        var node = snNPCNode.Instance() as NPC;
        node.state = new NPCState(node, parent, false);
        parent.runtime.entityRegistry.Add(node);
        return node;
    }

    public static Laborer CreateLaborer(IHaveRuntime parent)
    {
        var node = snLaborerNode.Instance() as Laborer;
        node.state = new NPCState(node, parent, false);
        parent.runtime.entityRegistry.Add(node);
        return node;
    }

    internal static Soldier CreateSoldier(IHaveRuntime parent)
    {
        var node = snSoldierNode.Instance() as Soldier;
        node.state = new NPCState(node, parent, false);
        parent.runtime.entityRegistry.Add(node);
        return node;
    }
}
using System;
using Godot;


public class Cottage : IStructure, IMenuState
{
    private StructureNode node;

    public string Name => "Cottage";
    public string Description => "Just a boring old cottage";
    public TickHandler tickHandler { get; private set; }
    public TargetingSystem Targeting { get; }

    public PlayerState player { get; private set; }




    public Cottage(PlayerState player)
    {
        this.player = player;
        tickHandler = new TickHandler();
        Targeting = new TargetingSystem();
    }
    public void ConfigureNode(StructureNode structureNode)
    {
        this.node = structureNode;
        OverrideModel();
    }

    public void RightClick(Vector2 position)
    {
        return;
    }

    protected PackedScene snModel => (PackedScene)ResourceLoader.Load("res://scenes/Models/TowerModel.tscn");

    protected void OverrideModel()
    {
        var modelNode = (Node2D)snModel.Instance();
        modelNode.Name = "Model";
        node.AddChild(modelNode);

        node.OverrideHitboxes();

        node.size = new Vector2(160, 480);
        node.Update();

    }

    private bool TryCreateNPC()
    {
        GD.Print("CREATING NPC");
        if (player.bank.food >= 5)
        {
            var npc = ActorFactory.CreateLaborer(node.runtime.World);
            tickHandler.AddOrder(new SpawnOrder<Laborer>(npc, node.runtime.World, node), 1);
            player.bank.food -= 5;
            return true;
        }
        else return false;
    }


    private bool TryCreateSoldier()
    {
        GD.Print("CREATING NPC");
        if (player.bank.food >= 100)
        {
            var npc = ActorFactory.CreateSoldier(node.runtime.World);
            tickHandler.AddOrder(new SpawnOrder<Soldier>(npc, node.runtime.World, node), 1);
            player.bank.food -= 100;
            return true;
        }
        else return false;
    }


    public void ConfigureButtons(PartialMenu partialMenu)
    {
        var createNPC = partialMenu.button1;
        createNPC.Hotkey = "Q";
        createNPC.Action = () => TryCreateNPC();

        createNPC.Visible = true;

        var createSoldier = partialMenu.button2;
        createSoldier.Hotkey = "W";
        createSoldier.Action = () => TryCreateSoldier();
        createSoldier.Visible = true;

        partialMenu.button3.Visible = true;
        partialMenu.button4.Visible = true;

    }


    public ITask GetFriendlyTask(BaseActorNode node)
    {
        throw new NotImplementedException();
    }

    public ITask GetHostileTask(BaseActorNode node)
    {
        throw new NotImplementedException();
    }

}

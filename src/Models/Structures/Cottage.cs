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
        OverrideSpriteColor();
    }

    public void RightClick(Vector2 position)
    {
        return;
    }

    private void OverrideSpriteColor()
    {
        var theme = new SpriteTheme();
        var defaultColor = theme.cEnemy;
        node.sprite.Modulate = theme.cCottage;
    }

    private bool TryCreateNPC()
    {
        GD.Print("CREATING NPC");

        var npc = ActorFactory.CreateNPC(node.runtime.World);
        tickHandler.AddOrder(new SpawnOrder(npc, node.runtime.World, node), 1);
        return true;

    }


    public void ConfigureButtons(PartialMenu partialMenu)
    {
        var createNPC = partialMenu.button1;
        createNPC.Hotkey = "Q";
        createNPC.Action = () => TryCreateNPC();

        createNPC.Visible = true;
        partialMenu.button2.Visible = true;
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

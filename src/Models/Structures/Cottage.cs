using System.Runtime.InteropServices.ComTypes;
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

    public Stat healthStat { get; set; } = Stat.Health(1000);


    public int Health => healthStat.current;
    public int MaxHealth => healthStat.standard;

    public StatusHandler statusHandler { get; set; }


    public Cottage(PlayerState player)
    {
        this.player = player;
        tickHandler = new TickHandler();
        Targeting = new TargetingSystem();
    }
    public void ConfigureNode(StructureNode structureNode)
    {
        this.node = structureNode;
        statusHandler = new StatusHandler(node);
        OverrideModel();
    }

    public void RightClick(Vector2 position)
    {
        return;
    }



    protected void OverrideModel()
    {

        return;

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

    public void AddStatus(eStatusEffect s, double duration)
    {
        statusHandler.AddStatus(StatusEffect.Create(s));
        tickHandler.AddOrder(new RemoveStatusOrder(node as ISufferStatusEffects, s), duration);
    }

    public bool HandleDamage(int damage)
    {
        healthStat.FlatChange(-1 * damage);
        return healthStat.current > 0;
    }
    public ITask GetFriendlyTask(BaseActorNode node, StructureNode self)
    {
        throw new NotImplementedException();
    }

    public ITask GetHostileTask(BaseActorNode node, StructureNode self)
    {
        if (node is ICanAttack)
        {
            return new AttackTask(node as ICanAttack, self);
        }
        return null;
    }

}

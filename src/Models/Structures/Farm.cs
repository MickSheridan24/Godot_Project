
using System;
using Godot;
using Mavisithor_Beaconizath.src.Interfaces;

public class Farm : IStructure, IMenuState
{
    private StructureNode node;

    public TickHandler tickHandler { get; private set; }
    public TargetingSystem Targeting { get; }

    public string Name => "Farm";
    public string Description => "Grows corn and the like";

    public PlayerState player { get; private set; }

    private Texture text = GD.Load<Texture>("res://assets/Farm.png");

    public Stat healthStat { get; set; } = Stat.Health(1000);
    public StatusHandler statusHandler { get; set; }

    public int Health => healthStat.current;
    public int MaxHealth => healthStat.standard;
    public Farm(PlayerState player)
    {
        tickHandler = new TickHandler();
        this.player = player;
        Targeting = new TargetingSystem();
    }
    public void ConfigureNode(StructureNode structureNode)
    {
        this.node = structureNode;
        OverrideModel();
        node.RemoveCollisions();
    }


    protected PackedScene snModel => (PackedScene)ResourceLoader.Load("res://scenes/Models/FarmModel.tscn");


    protected void OverrideModel()
    {
        var modelNode = (Node2D)snModel.Instance();
        node.AddChild(modelNode);

    }
    public Vector2 Position()
    {
        return node.GlobalPosition;
    }

    public void RightClick(Vector2 position)
    {
        return;
    }


    public void ConfigureButtons(PartialMenu partialMenu)
    {
        partialMenu.button1.Visible = false;
        partialMenu.button2.Visible = false;
        partialMenu.button3.Visible = false;
        partialMenu.button4.Visible = false;
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

    public bool HandleDamage(int damage)
    {
        healthStat.FlatChange(-1 * damage);
        return healthStat.current > 0;
    }
    public void AddStatus(eStatusEffect s, double duration)
    {
        statusHandler.AddStatus(StatusEffect.Create(s));
        tickHandler.AddOrder(new RemoveStatusOrder(node as ISufferStatusEffects, s), duration);
    }

}

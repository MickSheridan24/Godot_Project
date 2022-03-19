
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
        return node.Position;
    }

    public void RightClick(Vector2 position)
    {
        return;
    }

    private void OverrideSpriteColor()
    {
        var theme = new SpriteTheme();
        var defaultColor = theme.cEnemy;
        node.sprite.Modulate = theme.cFarm;
    }

    public void ConfigureButtons(PartialMenu partialMenu)
    {
        partialMenu.button1.Visible = false;
        partialMenu.button2.Visible = false;
        partialMenu.button3.Visible = false;
        partialMenu.button4.Visible = false;
    }

    public ITask GetFriendlyTask(BaseActorNode node)
    {
        if (node is ICanDoLabor)
        {
            return new StartFarmingTask(node as ICanDoLabor, this);
        }
        else return new DoNothingTask();
    }

    public ITask GetHostileTask(BaseActorNode node)
    {
        throw new System.NotImplementedException();
    }
}

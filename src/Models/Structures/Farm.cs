
using Godot;

public class Farm : IStructure, IMenuState
{
    private StructureNode node;

    public TickHandler tickHandler { get; private set; }
    public TargetingSystem Targeting { get; }

    public string Name => "Farm";
    public string Description => "Grows corn and the like";

    private Texture text = GD.Load<Texture>("res://assets/Farm.png");


    public Farm()
    {
        tickHandler = new TickHandler();

        Targeting = new TargetingSystem();
    }
    public void ConfigureNode(StructureNode structureNode)
    {
        this.node = structureNode;
        OverrideSpriteColor();
        node.RemoveCollisions();
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
        return new StartFarmingTask(node, this);
    }

    public ITask GetHostileTask(BaseActorNode node)
    {
        throw new System.NotImplementedException();
    }
}
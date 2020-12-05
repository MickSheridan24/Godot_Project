
using Godot;

public class Farm : IStructure, IMenuState
{
    private StructureNode node;

    public string Name => "Farm";
    public string Description => "Grows corn and the like";

    private Texture text = GD.Load<Texture>("res://assets/Farm.png");

    public void ConfigureNode(StructureNode structureNode)
    {
        this.node = structureNode;
        OverrideSpriteColor();
        node.RemoveCollisions();
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

    public void AddButtons(PartialMenu menu)
    {
        return;
    }
}
using Godot;


public class Cottage : IStructure
{
    private StructureNode node;

    public string Name => "Cottage";
    public string Description => "Just a boring old cottage";
    private Texture text = GD.Load<Texture>("res://assets/Cottage.png");

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
}

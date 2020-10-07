
using Godot;

public class Farm : IStructure
{
    private StructureNode node;

    private Texture text = GD.Load<Texture>("res://assets/Farm.png");

    public void ConfigureNode(StructureNode structureNode)
    {
        this.node = structureNode;
        node.sprite.SetTexture(text);
    }
}
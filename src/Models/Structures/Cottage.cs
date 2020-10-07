using Godot;


public class Cottage : IStructure
{
    private StructureNode node;

    private Texture text = GD.Load<Texture>("res://assets/Cottage.png");

    public void ConfigureNode(StructureNode structureNode)
    {
        this.node = structureNode;
        node.sprite.SetTexture(text);
    }
}

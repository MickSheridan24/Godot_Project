
using Godot;

public interface IStructure
{
    string Name { get; }
    string Description { get; }

    void ConfigureNode(StructureNode structureNode);
    void RightClick(Vector2 position);

}

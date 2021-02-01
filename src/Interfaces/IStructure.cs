
using Godot;

public interface IStructure
{
    string Name { get; }
    string Description { get; }

    void ConfigureNode(StructureNode structureNode);
    void RightClick(Vector2 position);


    TickHandler tickHandler { get; }
    TargetingSystem Targeting { get; }

    ITask GetFriendlyTask(BaseActorNode node);
    ITask GetHostileTask(BaseActorNode node);
}


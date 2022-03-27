
using Godot;

public interface IStructure : IHaveHealth
{
    string Name { get; }
    string Description { get; }

    void ConfigureNode(StructureNode structureNode);
    void RightClick(Vector2 position);


    TickHandler tickHandler { get; }
    TargetingSystem Targeting { get; }

    ITask GetFriendlyTask(BaseActorNode node, StructureNode structureNode);
    ITask GetHostileTask(BaseActorNode node, StructureNode structureNode);

    PlayerState player { get; }

    StatusHandler statusHandler { get; set; }

    void AddStatus(eStatusEffect s, double duration);

    bool HandleDamage(int damage);

}


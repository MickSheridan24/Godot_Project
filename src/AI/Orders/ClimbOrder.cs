
using Godot;

public class ClimbOrder : IOrder
{
    private IElevatable elevatable;
    private eCollisionLayers levelTo;

    public ClimbOrder(IElevatable elevatable, eCollisionLayers levelTo)
    {
        this.elevatable = elevatable;
        this.levelTo = levelTo;
    }

    public bool Execute()
    {
        elevatable.Elevate(levelTo);
        elevatable.CompleteClimb();
        elevatable.DisableFall(2);
        return true;
    }
}

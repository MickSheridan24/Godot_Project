using Godot;

public class VectorTarget : ITarget
{
    private Vector2 position { get; set; }

    public VectorTarget(Vector2 v)
    {
        position = v;
    }

    public Vector2 GetTargetPosition()
    {
        return position;
    }

}
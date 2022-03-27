
using Godot;

public interface IElevatable
{
    void SetCollisionLayerBit(int n, bool v);

    void SetCollisionMaskBit(int n, bool v);

    bool GetCollisionLayerBit(int n);

    bool GetCollisionMaskBit(int n);
    Vector2 Position { get; set; }
    Vector2 GlobalPosition { get; set; }

    void Elevate(eCollisionLayers level);

    bool isFallDisabled { get; set; }
    void CompleteClimb();
    void DisableFall(int v);
}

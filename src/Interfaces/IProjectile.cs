using Godot;

public interface IProjectile
{
    eProjectileType projectileType { get; set; }

    IProjectileNode node { get; set; }
    Vector2 direction { get; set; }
    Vector2 speed { get; set; }
    Vector2 start { get; set; }
    Vector2 range { get; set; }

    void HandleImpact(Area2D area);
    void HandleRayCollision();
    void ConfigureNode();
    void HandleProcess();
}

public enum eProjectileType
{
    FIREBALL,
    LIGHTNING
}
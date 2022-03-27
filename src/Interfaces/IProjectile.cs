using Godot;

public interface IProjectile
{
    eProjectileType projectileType { get; set; }

    IProjectileNode node { get; set; }
    Vector2 direction { get; set; }
    Vector2 speed { get; set; }
    Vector2 start { get; set; }
    Vector2 range { get; set; }

    int? duration { get; set; }

    int damage { get; set; }

    void HandleImpact(KinematicCollision2D collision);
    void HandleRayCollision();
    void ConfigureNode();
    void HandleProcess();
}

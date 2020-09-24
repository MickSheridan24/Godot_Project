using Godot;

public interface IProjectile
{
    eProjectileType projectileType { get; }
    Vector2 direction { get; set; }
    Vector2 speed { get; set; }
    Vector2 start { get; set; }
    Vector2 maxDistance { get; set; }

    void HandleImpact(Area2D area, IProjectileNode node);

}

public enum eProjectileType
{
    FIREBALL,
    LIGHTNING
}
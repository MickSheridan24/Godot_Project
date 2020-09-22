using Godot;

public interface IProjectile
{
    eProjectileType projectileType { get; }
    Vector2 direction { get; set; }
    Vector2 speed { get; set; }
    Vector2 start { get; set; }
    Vector2 maxDistance { get; set; }

}

public enum eProjectileType
{
    FIREBALL
}
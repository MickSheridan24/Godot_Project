using Godot;

public class ProjectileEntity
{
    public eProjectileType projectileType { get; set; }
    public Vector2 direction { get; set; }
    public Vector2 speed { get; set; }
    public Vector2 maxDistance { get; set; }
}

public enum eProjectileType
{
    FIREBALL
}
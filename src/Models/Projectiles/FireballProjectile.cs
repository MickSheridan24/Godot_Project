using Godot;

public class FireballProjectile : IProjectile
{
    public eProjectileType projectileType { get => eProjectileType.FIREBALL; }
    public Vector2 start { get; set; }
    public Vector2 direction { get; set; }
    public Vector2 speed { get; set; }
    public Vector2 maxDistance { get; set; }

    public FireballProjectile()
    {
        speed = new Vector2(10, 10);
        maxDistance = new Vector2(300, 300);
    }
}

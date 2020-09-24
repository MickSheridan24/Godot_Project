using Godot;

public class LightningProjectile : IProjectile
{
    public eProjectileType projectileType { get => eProjectileType.LIGHTNING; }
    public Vector2 start { get; set; }
    public Vector2 direction { get; set; }
    public Vector2 speed { get; set; }
    public Vector2 maxDistance { get; set; }

    public LightningProjectile()
    {
        speed = new Vector2(30, 30);
        maxDistance = new Vector2(600, 600);
    }

    public void HandleImpact(Area2D area, IProjectileNode node)
    {
        node.QueueFree();
    }
}

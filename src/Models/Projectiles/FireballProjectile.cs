using Godot;

public class FireballProjectile : ProjectileBase, IProjectile
{
    public FireballProjectile()
    {
        projectileType = eProjectileType.FIREBALL;
        speed = new Vector2(10, 10);
        range = new Vector2(300, 300);
    }
    public void HandleImpact(Area2D area)
    {
        node.ExecQueueFree();
    }

    public void ConfigureNode()
    {
        node.sprite.Set("modulate", theme.cFireball);
    }

    public void HandleRayCollision()
    {

    }

    public void HandleProcess()
    {

    }
}

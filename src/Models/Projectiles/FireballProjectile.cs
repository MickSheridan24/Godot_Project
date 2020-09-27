using Godot;

public class FireballProjectile : ProjectileBase, IProjectile
{
    public FireballProjectile()
    {
        projectileType = eProjectileType.FIREBALL;
        speed = new Vector2(400, 400);
        range = new Vector2(300, 300);

        damage = 300;
    }
    public void HandleImpact(Area2D area)
    {
        TryDamage(area?.GetParent(), eDamageType.FIRE);

        foreach (var effected in FindEffected<IDamageable>(area))
        {
            (effected as IDamageable).Damage(damage / 2, eDamageType.FIRE);
        }

        node.ExecQueueFree();
    }

    public void ConfigureNode()
    {
        node.sprite.Set("modulate", theme.cFireball);
        SetEffectRadius(200);
    }

    public void HandleRayCollision()
    {

    }

    public void HandleProcess()
    {

    }
}

using Godot;

public class FireballProjectile : ProjectileBase, IProjectile
{

    public FireballProjectile()
    {
        projectileType = eProjectileType.FIREBALL;
        speed = new Vector2(400, 400);
        range = new Vector2(500, 500);

        damage = 300;
    }
    public void HandleImpact(KinematicCollision2D collision)
    {

        var collider = collision.Collider as Node;
        TryDamage(collider, eDamageType.FIRE);

        foreach (var effected in FindEffected<IDamageable>(collider))
        {
            (effected as IDamageable).Damage(damage / 2, eDamageType.FIRE);
        }

        node.ExecQueueFree();
    }

    public void ConfigureNode()
    {
        SetEffectRadius(FireballSpell.effectRadius);
        var tex = GD.Load<Texture>("res://assets/Fireball.png");
        node.sprite.Texture = tex;
    }

    public void HandleRayCollision()
    {

    }

    public void HandleProcess()
    {

    }
}

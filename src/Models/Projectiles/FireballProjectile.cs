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
        SetEffectRadius(200);
        //node.sprite.Set("load_path", "res://.import/Fireball.png-8cbc75beff1786c7589f78ccd41ff859.stex");
        //     node.sprite.Set("hframes", 2);
        //     node.sprite.DrawTextureRect(node.sprite.Texture, new Rect2(Vector2.Zero, new Vector2(64, 32)), false);
    }

    public void HandleRayCollision()
    {

    }

    public void HandleProcess()
    {

    }
}

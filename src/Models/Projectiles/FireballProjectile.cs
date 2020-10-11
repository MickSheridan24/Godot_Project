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
        SetEffectRadius(FireballSpell.effectRadius);
        var tex = GD.Load<Texture>("res://assets/Fireball.png");
        node.sprite.SetTexture(tex);
    }

    public void HandleRayCollision()
    {

    }

    public void HandleProcess()
    {

    }
}

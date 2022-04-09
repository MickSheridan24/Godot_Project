
using Godot;

public class BlizzardProjectile : ProjectileBase, IProjectile
{


    public void ConfigureNode()
    {
        projectileType = eProjectileType.BLIZZARD;
        duration = 1000;
        var dest = start;
        SetEffectRadius(700);
        node.sprite.Set("modulate", theme.Blizzard);
    }

    public void HandleImpact(KinematicCollision2D collision)
    {
        return;
    }

    public void HandleProcess()
    {
        if (duration > 0)
        {
            var colliders = FindEffected<ICanFreeze>();
            foreach (var item in colliders)
            {
                (item as ICanFreeze).AddFreezingEffect(10);
            }

        }
        duration--;
    }

    public void HandleRayCollision()
    {
        return;
    }
}

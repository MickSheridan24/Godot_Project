
using Godot;

public class BlizzardProjectile : ProjectileBase, IProjectile
{


    public void ConfigureNode()
    {
        duration = 100;
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
    }

    public void HandleRayCollision()
    {
        throw new System.NotImplementedException();
    }
}

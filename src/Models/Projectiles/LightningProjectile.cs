using System.Collections.Generic;
using System.Linq;
using Godot;

public class LightningProjectile : ProjectileBase, IProjectile
{
    private int spawncount;
    private bool isLaunching;

    public LightningProjectile(int count)
    {
        projectileType = eProjectileType.LIGHTNING;
        speed = new Vector2(0, 0);
        range = new Vector2(600, 600);
        spawncount = count;
        isLaunching = false;
    }

    public void HandleImpact(Area2D area)
    {
        var target = area?.GetParent();
        isLaunching = false;

        TryDamage(target, eDamageType.ELECTRIC);
        TryConduct(target, area);

        node.ExecQueueFree();
    }

    public void HandleProcess()
    {
        if (!isLaunching)
        {
            spawncount--;
            if (spawncount <= 0)
            {
                node.ExecQueueFree();
            }
        }
    }

    private void TryConduct(Node target, Area2D area)
    {
        if (target != null && target is IConductElectricity)
        {
            var conductor = (target as IConductElectricity);
            var conduit = FindConduit(conductor, area);
            conductor.AddStatusEffect(new JoltedEffect(4));
            if (conduit != null)
            {
                Conduct(conductor, conduit);
            }
        }
    }

    private IConductElectricity FindConduit(IConductElectricity target, Area2D area)
    {
        var conduits = FindEffected<IConductElectricity>(area)
                                        .Select(i => i as IConductElectricity)
                                        .Where(a => !a.HasStatusEffect(eStatusEffect.JOLTED))

                                        .Select(c => new
                                        {
                                            conduit = c,
                                            prox = c.GetTargetPosition().ProximityTo(target.GetTargetPosition()).GetScale()
                                        });

        return conduits.OrderBy(c => c.prox).Select(p => p.conduit).FirstOrDefault();
    }

    private void Conduct(IConductElectricity conductor, IConductElectricity conduit)
    {
        var dir = conductor.GetTargetPosition().DirectionTo(conduit.GetTargetPosition());
        CreateConductor(dir, conductor.GetTargetPosition());
    }





    private void CreateConductor(Vector2 v, Vector2 p)
    {
        var newProj = new LightningProjectile(5)
        {
            direction = v,
            start = p
        };
        node.runtime.World.CreateProjectile(newProj, node.caster);
    }

    public void HandleRayCollision()
    {
        var collider = node.raycast.GetCollider() as Node2D;

        var isConductable = collider?.GetParent<IConductElectricity>() != null;


        if (collider != null && isConductable)
        {
            node.speed = new Vector2(1000, 1000);
            isLaunching = true;
        }
    }

    public void ConfigureNode()
    {
        var dest = start + node.direction * range;
        node.raycast.SetCastTo(dest);

        node.sprite.Set("modulate", theme.cLightning);

        (node.effectRadius.GetNode<CollisionShape2D>("CollisionShape2D").GetShape() as CircleShape2D).Radius = 100;


    }
}

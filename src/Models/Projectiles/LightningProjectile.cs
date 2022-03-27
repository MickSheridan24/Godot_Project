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
        range = new Vector2(1600, 1600);
        spawncount = count;
        isLaunching = false;
    }

    public void HandleImpact(KinematicCollision2D collision)
    {
        var collider = collision.Collider as Node;

        isLaunching = false;

        TryDamage(collider, eDamageType.ELECTRIC);
        TryConduct(collider);

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

    private void TryConduct(Node target)
    {
        if (target != null && target is IConductElectricity && spawncount > 0)
        {
            var conductor = (target as IConductElectricity);
            conductor.AddJoltedEffect(2);
            var conduit = FindConduit(conductor);
            if (conduit != null)
            {
                Conduct(conductor, conduit);
            }
        }
    }

    private IConductElectricity FindConduit(IConductElectricity target)
    {
        var conduits = FindEffected<IConductElectricity>(target as Node)
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
        var dir = conductor.Position.DirectionTo(conductor.ToLocal(conduit.GlobalPosition));
        CreateConductor(dir, conductor.Position);
    }





    private void CreateConductor(Vector2 v, Vector2 p)
    {
        var newProj = new LightningProjectile(spawncount - 1)
        {
            direction = v,
            start = p,
            damage = damage
        };
        node.runtime.World.CreateProjectile(newProj, node.caster);
    }

    public void HandleRayCollision()
    {
        var collider = node.raycast.GetCollider() as Node2D;

        var isConductable = collider?.GetParent() is IConductElectricity;


        if (collider != null && isConductable)
        {
            node.speed = new Vector2(1500, 1500);
            isLaunching = true;
        }
    }

    public void ConfigureNode()
    {
        var dest = start + node.direction * range;
        node.raycast.CastTo = dest;
        SetEffectRadius(LightningSpell.effectRadius);
        node.sprite.Set("modulate", theme.cLightning);



    }
}

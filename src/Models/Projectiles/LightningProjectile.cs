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
        var conductor = area?.GetParent();
        isLaunching = false;

        if (conductor != null && conductor is IConductElectricity)
        {
            var iConductor = (conductor as IConductElectricity);
            iConductor.EnterDamageState(10);
            iConductor.AddStatusEffect(new JoltedEffect(4));
            Conduct(iConductor);
        }
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
    private void Conduct(IConductElectricity exclude)
    {
        CreateConductor(Vector2.Up, exclude.GetTargetPosition());
        CreateConductor(Vector2.Down, exclude.GetTargetPosition());
        CreateConductor(Vector2.Left, exclude.GetTargetPosition());
        CreateConductor(Vector2.Right, exclude.GetTargetPosition());
        CreateConductor(Vector2.Up + Vector2.Left, exclude.GetTargetPosition());
        CreateConductor(Vector2.Up + Vector2.Right, exclude.GetTargetPosition());
        CreateConductor(Vector2.Down + Vector2.Left, exclude.GetTargetPosition());
        CreateConductor(Vector2.Down + Vector2.Right, exclude.GetTargetPosition());
    }



    private void CreateConductor(Vector2 v, Vector2 p)
    {
        var newProj = new LightningProjectile(2)
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
            node.speed = new Vector2(200, 200);
            isLaunching = true;
        }
    }

    public void ConfigureNode()
    {
        var dest = start + node.direction * range;
        node.raycast.SetCastTo(dest);

        node.sprite.Set("modulate", theme.cLightning);
    }
}

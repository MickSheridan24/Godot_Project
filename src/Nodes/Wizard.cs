using Godot;
using System;

public class Wizard : BaseActorNode, ISelectable, IHaveHealth, IMove, IHaveRuntime, ICaster,
                      IElevatable, ITarget, IDamageable, ISufferStatusEffects, IHaveTarget
{


    private AnimationPlayer animation => sprite.GetNode<AnimationPlayer>("AnimationPlayer");
    public Vector2 speed => state.speed.current.ToVector();

    private PackedScene snSimpleProjectile => (PackedScene)ResourceLoader.Load("res://scenes/SimpleProjectile.tscn");

    public int Health => state.health.current;
    public int MaxHealth => state.health.standard;
    public AimLine aimLine => GetNode<AimLine>("AimLine");

    public string EntityName => state.Name;
    public string Description => state.Description;
    public Vector2 size => new Vector2(32, 32);



    //overrides

    public override void _Process(float d)
    {
        state.elevationHandler.HandleElevation();
        OverrideSpriteColor();
        aimLine.dest = runtime?.RightTarget?.GetTargetPosition() ?? aimLine.dest;
        aimLine.Update();
    }



    //ISelectable

    public TargetingSystem Targeting { get => state.Targeting; }
    public void RightClick(InputEventMouseButton mouse)
    {
        var dest = GetGlobalMousePosition();
        SetDestination(dest);
    }

    public Rect2 GetSelectionArea()
    {
        return new Rect2(GlobalPosition - size / 2, size);
    }

    //IHaveTarget 

    public bool CanTarget(ITarget target)
    {
        return true; //Can target anything
    }

    public void SetLeftTarget(ITarget target)
    {
        Targeting.SetLeftTarget(target);
    }

    public void SetRightTarget(ITarget target)
    {
        Targeting.SetRightTarget(target);
    }

    public void ClearTargets()
    {
        Targeting.Clear();
    }


    //IMove

    public bool CanMove()
    {
        return !runtime.IsCasting;
    }

    //Projectile Creation

    public void CreateProjectile(IProjectile projectileDetails)
    {
        switch (projectileDetails.projectileType)
        {
            case eProjectileType.FIREBALL:
                CreateSimpleProjectile(projectileDetails);
                break;
            case eProjectileType.LIGHTNING:
                CreateSimpleProjectile(projectileDetails);
                break;
            default:
                break;
        }
    }

    private void CreateSimpleProjectile(IProjectile projectileDetails)
    {
        var projectile = (SimpleProjectile)snSimpleProjectile.Instance();
        projectile.Config(projectileDetails, this);
        AddChild(projectile);
    }


    //private
    private void OverrideSpriteColor()
    {
        var defaultColor = runtime.currentSelection == this ? theme.selected : theme.unselected;
        sprite.Modulate = state.statusHandler.HasStatus(eStatusEffect.INTANGIBLE) ? theme.cEnemyHit : defaultColor;
    }

    private void SetTarget(ITarget t)
    {
        runtime.SetTarget(t);
    }
    public void HandleCollision(KinematicCollision2D collision)
    {
        var collider = collision.GetCollider();

        if (collider is Enemy)
        {
            Damage((collider as Enemy).GetDamage, eDamageType.PHYSICAL);
        }
    }

    public void Damage(int power, eDamageType type)
    {
        if (!state.statusHandler.HasStatus(eStatusEffect.INTANGIBLE))
        {
            switch (type)
            {
                default:
                    state.AddStatus(eStatusEffect.INTANGIBLE, 2);
                    TakeDamage(power);
                    break;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (!state.HandleDamage(damage))
        {
            ExecQueueFree();
        }
    }


    public void ExecQueueFree()
    {
        runtime.wizardNode = null;
        CallDeferred("queue_free");
    }


    public void CompleteClimb()
    {
        return;
    }

    public void DisableFall(int v)
    {
        return;
    }



    //ITarget

    public Vector2 GetTargetPosition()
    {
        if (IsInsideTree()) return Position;
        else return Vector2.Zero;
    }


    public void BecomeIntangible()
    {
        SetCollisionLayerBit((int)eCollisionLayers.ENTITY, false);
        SetCollisionLayerBit((int)eCollisionLayers.FRIENDLY, false);
        SetCollisionLayerBit((int)eCollisionLayers.INTANGIBLE, true);
        sprite.Modulate = new SpriteTheme().cEnemyHit;
    }

    public void EndIntangible()
    {
        SetCollisionLayerBit((int)eCollisionLayers.ENTITY, true);
        SetCollisionLayerBit((int)eCollisionLayers.FRIENDLY, true);
        SetCollisionLayerBit((int)eCollisionLayers.INTANGIBLE, false);
    }

    public void RemoveEffect(eStatusEffect eff)
    {
        state.statusHandler.RemoveStatus(eff);
    }

    public PartialMenu GetPartial()
    {
        return null;
    }

    public IMenuState GetMenuState()
    {
        return null;
    }
}


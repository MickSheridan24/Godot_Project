using Godot;
using System;

public class Wizard : KinematicBody2D, ISelectable, IHaveHealth, IMove, IHaveRuntime, ICaster,
                      IElevatable, ITarget, IDamageable, ISufferStatusEffects
{

    //props

    public Runtime runtime => GetParent<IHaveRuntime>().runtime;
    public IWizardState state => runtime.WizardState;
    public AimLine aimLine => GetNode<AimLine>("AimLine");
    public Moveable moveable;
    private WeakRef weakref;

    public string EntityName => state.Name;
    public string Description => state.Description;
    private Sprite sprite => GetNode<Sprite>("Sprite");
    private AnimationPlayer animation => sprite.GetNode<AnimationPlayer>("AnimationPlayer");
    public Area2D body => GetNode<Area2D>("Area2D");
    public Vector2 spritePosition => sprite.Position;

    public SpriteTheme theme => new SpriteTheme();
    public Vector2 destination { get; set; }
    public Vector2 speed => state.speed.current.ToVector();
    public bool MovingTarget { get; set; }

    public bool isFallDisabled { get; set; }
    private PackedScene snSimpleProjectile => (PackedScene)ResourceLoader.Load("res://scenes/SimpleProjectile.tscn");

    public int Health => state.health.current;
    public int MaxHealth => state.health.standard;



    //overrides
    public override void _Ready()
    {
        moveable = new Moveable(this);
        destination = Position;
        MovingTarget = true;
        isFallDisabled = false;
        weakref = WeakRef(this);
    }

    public override void _Process(float d)
    {
        state.elevationHandler.HandleElevation();

        OverrideSpriteColor();
        aimLine.dest = runtime?.RightTarget?.GetTargetPosition() ?? aimLine.dest;
        aimLine.Update();
    }
    private void HandleAnimation()
    {
        var dir = Position.DirectionTo(destination).ToEightDir();
        var anim = "";
        if (dir == Vector2.Down)
        {
            sprite.Frame = 0;
            anim = "Walk_Down";
        }
        else if (dir == Vector2.Right)
        {
            sprite.Frame = 2;
            anim = "Walk_Right";
        }
        else if (dir == Vector2.Up)
        {
            sprite.Frame = 4;
            anim = "Walk_Up";
        }
        else if (dir == Vector2.Left)
        {
            sprite.Frame = 6;
            anim = "Walk_Left";
        }
        if (moveable.moving && animation.CurrentAnimation == "" && anim != "")
        {
            GD.Print(anim);
            animation.Play(anim, -1, 1000);
        }
        else if (animation.IsPlaying())
        {
            animation.Stop();
        }
    }



    public override void _PhysicsProcess(float delta)
    {
        HandleMove(delta);

        state.tickHandler.Tick();

    }

    //ISelectable
    public void Select()
    {
        runtime.currentSelection = this;
    }
    public void RightClick(InputEventMouseButton mouse)
    {
        var dest = GetGlobalMousePosition();
        SetDestination(dest);
    }

    //IMove
    public void HandleMove(float d)
    {
        moveable.HandleMove(d);
    }

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
    private void SetDestination(Vector2 position)
    {
        destination = position;
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
    public void Elevate(eCollisionLayers level)
    {
        state.elevationHandler.HandleElevation((int)level);
    }


    //ITarget
    public bool IsFreed()
    {
        return weakref.GetRef() == null;
    }

    public Vector2 GetTargetPosition()
    {
        if (IsInsideTree()) return Position;
        else return Vector2.Zero;
    }
    public void BecomeIntangible()
    {
        body.SetCollisionLayerBit((int)eCollisionLayers.ENTITY, false);
        body.SetCollisionLayerBit((int)eCollisionLayers.FRIENDLY, false);
        body.SetCollisionLayerBit((int)eCollisionLayers.INTANGIBLE, true);
        sprite.Modulate = new SpriteTheme().cEnemyHit;
    }

    public void EndIntangible()
    {
        body.SetCollisionLayerBit((int)eCollisionLayers.ENTITY, true);
        body.SetCollisionLayerBit((int)eCollisionLayers.FRIENDLY, true);
        body.SetCollisionLayerBit((int)eCollisionLayers.INTANGIBLE, false);
    }

    public void RemoveEffect(eStatusEffect eff)
    {
        state.statusHandler.RemoveStatus(eff);
    }

    public Rect2 GetSelectionArea()
    {
        return new Rect2(GetGlobalMousePosition(), sprite.RegionRect.Size);
    }
}


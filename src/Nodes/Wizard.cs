using Godot;
using System;

public class Wizard : KinematicBody2D, ISelectable, IMove, IHaveRuntime, ICaster, IElevatable, ITarget
{

    //props

    public Runtime runtime => GetParent<IHaveRuntime>().runtime;
    public WizardState state => runtime.WizardState;
    public AimLine aimLine => GetNode<AimLine>("AimLine");
    public Moveable moveable;
    private Sprite sprite => GetNode<Sprite>("Sprite");
    public Area2D body => GetNode<Area2D>("Area2D");
    public Vector2 spritePosition => sprite.Position;
    private Color unselected => new Color("#a822dd");
    private Color selected => new Color("a866ff");
    public Vector2 destination { get; set; }
    public Vector2 speed => state.speed.current.ToVector();
    public bool MovingTarget { get; set; }


    public bool isFallDisabled { get; set; }
    private PackedScene snSimpleProjectile => (PackedScene)ResourceLoader.Load("res://scenes/SimpleProjectile.tscn");




    //overrides
    public override void _Ready()
    {
        moveable = new Moveable(this);
        destination = Position;
        MovingTarget = true;
        isFallDisabled = false;
    }

    public override void _Process(float d)
    {
        state.elevationHandler.HandleElevation();
        OverrideSpriteColor(runtime.currentSelection == this ? selected : unselected);
        aimLine.dest = runtime?.RightTarget?.GetTargetPosition() ?? aimLine.dest;
        aimLine.Update();
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
    private void OverrideSpriteColor(Color c)
    {
        sprite.Modulate = c;
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
    }

    public void CompleteClimb()
    {
        return;
    }

    public void Elevate(eCollisionLayers level)
    {
        state.elevationHandler.HandleElevation((int)level);
    }

    public void DisableFall(int v)
    {
        return;
    }

    public Vector2 GetTargetPosition()
    {
        return Position;
    }
}


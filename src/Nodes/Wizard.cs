using Godot;
using System;

public class Wizard : KinematicBody2D, ISelectable, IMove, IHaveRuntime, ICaster, IElevatable
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
    public Vector2 speed => state.moveSpeed;


    private PackedScene snSimpleProjectile => (PackedScene)ResourceLoader.Load("res://scenes/SimpleProjectile.tscn");


    //overrides
    public override void _Ready()
    {
        moveable = new Moveable(this);
        destination = Position;
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
        //   runtime.SetWorldTarget(dest);
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

    public void HandleMove()
    {
        throw new NotImplementedException();
    }
}

using Godot;
using System;

public class Wizard : Node2D, ISelectable, IMove
{

    //props

    public Runtime runtime => GetParent<IHaveRuntime>().runtime;
    public WizardState state => runtime.WizardState;
    public Moveable moveable;
    private Sprite sprite => GetNode<Sprite>("Sprite");

    public Vector2 position => sprite.Position;
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
    public override void _Process(float delta)
    {

        OverrideSpriteColor(runtime.currentSelection == this ? selected : unselected);

        HandleMove();
    }

    //signal handlers
    public void _onInputEvent(Node n, InputEvent @event, int idx)
    {
        if (@event is InputEventMouseButton && (@event as InputEventMouseButton).ButtonIndex == (int)ButtonList.Left)
        {
            runtime.currentSelection = this;
        }
    }

    //ISelectable
    public void Select()
    {
        runtime.currentSelection = this;
    }
    public void RightClick(InputEventMouseButton mouse)
    {
        SetDestination(mouse.Position);
        SetTarget(mouse.Position);
    }



    //IMove
    public void HandleMove()
    {
        moveable.HandleMove();
    }

    public bool CanMove()
    {
        return !runtime.IsCasting;
    }

    //Projectile Creation

    public void CreateProjectile(ProjectileEntity projectileDetails)
    {
        switch (projectileDetails.projectileType)
        {
            case eProjectileType.FIREBALL:
                CreateSimpleProjectile(projectileDetails);
                break;
            default:
                break;
        }
    }

    private void CreateSimpleProjectile(ProjectileEntity projectileDetails)
    {
        var projectile = (SimpleProjectile)snSimpleProjectile.Instance();
        projectile.Config(Position, projectileDetails);
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
    private void SetTarget(Vector2 position)
    {
        runtime.currentTarget = new SpellTarget()
        {
            vTarget = position,
            isVTarget = true
        };
    }

}

using Godot;
using System;

public class SimpleProjectile : KinematicBody2D, IProjectileNode, IMove
{
    private IProjectile state;
    public ICaster caster { get; set; }
    public Runtime runtime => GetParent<IHaveRuntime>().runtime;
    public eProjectileType projectileType { get; private set; }
    public Vector2 speed { get; set; }
    public Vector2 direction { get; set; }
    public Vector2 range { get; set; }
    public Sprite sprite => GetNode<Sprite>("Sprite");
    public RayCast2D raycast => GetNode<RayCast2D>("RayCast2D");
    public SpriteTheme theme => new SpriteTheme();

    public Vector2 destination { get; set; }


    private Vector2 remainingDistance;
    private Moveable moveable;

    public void Config(IProjectile projectileDetails, ICaster wiz)
    {
        projectileDetails.node = this;



        Position = projectileDetails.start;
        state = projectileDetails;

        speed = state.speed;
        direction = state.direction;
        range = state.range;

        projectileType = projectileDetails.projectileType;
        remainingDistance = range;

        caster = wiz;

        projectileDetails.ConfigureNode();

    }

    public override void _PhysicsProcess(float d)
    {
        destination = speed * direction;
        HandleMove();
        HandleRayCast();
        HandleOther();
    }

    private void HandleOther()
    {
        state.HandleProcess();
    }

    private void HandleRayCast()
    {
        if (raycast.IsColliding())
        {
            state.HandleRayCollision();
        }
    }

    private void HandleMove()
    {
        var move = MoveAndCollide(destination);

        if (move is KinematicCollision2D)
        {
            HandleCollision(move);
        }
        remainingDistance -= speed;
        if (remainingDistance <= Vector2.Zero)
        {
            QueueFree();
        }
    }

    //Signal Handlers 
    public void HandleCollision(KinematicCollision2D collision)
    {
        var isWizard = (collision.Collider as Area2D).GetParent() as Wizard;
        if (isWizard != caster)
        {
            state.HandleImpact(collision.Collider as Area2D);
        }
    }
    public void _onBodyEntered(Area2D area)
    {
        var isWizard = area.GetParent() as Wizard;
        if (isWizard != caster)
        {
            state.HandleImpact(area);
        }
    }


    public void ExecQueueFree()
    {
        CallDeferred("free");
    }

    void IMove.HandleMove()
    {
        throw new NotImplementedException();
    }

    public bool CanMove()
    {
        return true;
    }
}

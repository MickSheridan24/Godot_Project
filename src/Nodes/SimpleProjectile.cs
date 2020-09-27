using Godot;
using System;

public class SimpleProjectile : KinematicBody2D, IProjectileNode
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

    public Area2D effectRadius => GetNode<Area2D>("EffectRadius");


    public Vector2 destination { get; set; }


    private Vector2 remainingDistance;

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

    public override void _Process(float d)
    {
        HandleOther();
        HandleRayCast();
    }
    public override void _PhysicsProcess(float d)
    {
        HandleMove(d);
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

    private void HandleMove(float d)
    {
        destination = speed * direction * d;
        var move = MoveAndCollide(destination);

        remainingDistance -= speed * d;
        if (remainingDistance <= Vector2.Zero)
        {
            QueueFree();
        }
    }

    //Signal Handlers 

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


    public bool CanMove()
    {
        return true;
    }
}

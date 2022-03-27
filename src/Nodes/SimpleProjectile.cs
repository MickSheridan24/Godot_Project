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
    private int? duration;

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

        duration = projectileDetails.duration;

        caster = wiz;

        projectileDetails.ConfigureNode();

    }

    public override void _Ready()
    {
        HandlePoint();
    }
    public override void _Process(float d)
    {
        HandleOther();
        HandleRayCast();
    }
    public override void _PhysicsProcess(float d)
    {
        if (remainingDistance > Vector2.Zero)
        {
            HandleMove(d);
        }
    }

    private void HandlePoint()
    {
        sprite.Rotate(direction.Angle() + (float)(Math.PI / 2));
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
        var collision = MoveAndCollide(destination);

        if (collision != null)
        {
            state.HandleImpact(collision);
            QueueFree();
        }
        else
        {
            remainingDistance -= speed * d;
            if (remainingDistance <= Vector2.Zero)
            {
                if (duration == null)
                {
                    ExecQueueFree();
                }
            }
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

using Godot;
using System;

public class Enemy : Node2D, IMove, ITarget, IProjectileTarget, IHaveRuntime
{

    public Sprite sprite => GetNode<Sprite>("Sprite");
    public Runtime runtime => GetParent<IHaveRuntime>().runtime;

    public EnemyState state { get; private set; }

    private Moveable moveable;

    public Vector2 destination { get; set; }

    public Vector2 speed => new Vector2(5, 5);
    public bool MovingTarget { get; set; }
    private Highlight highlight => GetNode<Highlight>("Highlight");

    public override void _Ready()
    {
        destination = Position;
        sprite.Modulate = new SpriteTheme().cEnemy;
        moveable = new Moveable(this);
        MovingTarget = true;
        highlight.position = Vector2.Zero;
    }

    public override void _Process(float d)
    {
        if (state == null)
        {
            state = runtime.CreateEnemyState(this);
        }
        state.RequestAction().Execute();
        highlight.Visible = runtime.currentTarget == this;
    }


    public override void _Input(InputEvent e)
    {
        var rClick = (e as InputEventMouseButton);
        if (rClick?.ButtonIndex == (int)ButtonList.Right && rClick.IsPressed() &&
        runtime.currentSelection != null && !rClick.IsEcho() && rClick.Position.InBounds(Position - new Vector2(16, 16), Position + new Vector2(16, 16)))
        {
            runtime.currentTarget = this;
            GetTree().SetInputAsHandled();
        }

    }

    public void HandleMove()
    {
        moveable.HandleMove();
    }

    public bool CanMove()
    {
        return true;
    }

    //IProjectileTarget
    public void HandleImpact(SimpleProjectile projectile)
    {
        switch (projectile.projectileType)
        {
            case eProjectileType.FIREBALL:
                if (!state.HandleDamage(300))
                {
                    QueueFree();
                }
                else
                {
                    GD.Print(state.health);
                }
                break;
            default: break;
        }

    }

    //SignalHandlers

    public void _onBodyEntered(Area2D body)
    {
        GD.Print("HIT");
        if (body is SimpleProjectile)
        {
            HandleImpact(body as SimpleProjectile);
        }
    }


    //ITarget
    public Vector2 GetTargetPosition()
    {
        return Position;
    }
}

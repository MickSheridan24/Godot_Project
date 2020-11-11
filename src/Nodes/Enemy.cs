using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Enemy : KinematicBody2D, IElevatable, IMove, ITarget, IDamageable, IHaveRuntime,
                     IConductElectricity, IFreeable, ISufferStatusEffects, IHaveHealth
{
    public string name { get; set; }
    public Sprite sprite => GetNode<Sprite>("Sprite");
    public AnimationPlayer animation => sprite.GetNode<AnimationPlayer>("AnimationPlayer");
    public Runtime runtime => GetParent<IHaveRuntime>().runtime;
    public EnemyState state { get; private set; }
    private Moveable moveable;
    public Vector2 destination { get; set; }
    public Vector2 speed { get; set; }
    public bool MovingTarget { get; set; }
    public Area2D body => GetNode<Area2D>("Area2D");
    private int DamageStateCounter;

    private IOrder order;
    private WeakRef weakref;


    private Highlight rightHighlight => GetNode<Highlight>("RightHighlight");
    private Highlight leftHighlight => GetNode<Highlight>("LeftHighlight");
    public int Health => state.health.current;
    public int MaxHealth => state.health.standard;

    public int GetDamage => state.damage.current;

    public bool isFallDisabled { get; set; }



    public override void _Ready()
    {
        destination = Position;
        isFallDisabled = false;
        speed = new Vector2(80, 80);
        moveable = new Moveable(this);
        MovingTarget = true;
        rightHighlight.position = Vector2.Zero;
        leftHighlight.position = Vector2.Zero;
        DamageStateCounter = 0;

        rightHighlight.color = new UITheme().cAccent;
        leftHighlight.color = new UITheme().cBlue;


        weakref = WeakRef(this);
    }

    public override void _Process(float d)
    {
        rightHighlight.Visible = runtime.RightTarget == this;
        leftHighlight.Visible = runtime.LeftTarget == this;
        InitState();
        state.elevationHandler.HandleElevation();
        state.statusHandler.HandleStatuses();

        OverrideSpriteColor();

        //HandleAnimation();
    }
    public override void _PhysicsProcess(float d)
    {
        order = state?.RequestAction(d);
        state?.Tick();
        if (order != null)
        {
            order.Execute();
            order = null;
        }
    }

    private void InitState()
    {
        if (state == null)
        {
            state = runtime.CreateEnemyState(this);
        }
    }

    public override void _Input(InputEvent e)
    {
        var pos = GetGlobalMousePosition();
        var rClick = (e as InputEventMouseButton);
        if (e.RightClickJustPressed() && pos.InBounds(Position - new Vector2(25, 25), Position + new Vector2(25, 25)))
        {
            runtime.SetRightTarget(this);
            GetTree().SetInputAsHandled();
        }

        if (e.LeftClickJustPressed() && pos.InBounds(Position - new Vector2(25, 25), Position + new Vector2(25, 25)))
        {
            runtime.SetLeftTarget(this);
            GetTree().SetInputAsHandled();
        }
    }

    public void HandleMove(float d)
    {
        moveable.HandleMove(d);
    }

    public bool CanMove()
    {
        return state.CanMove();
    }

    //IDamageable
    public void Damage(int power, eDamageType type)
    {
        switch (type)
        {
            default:
                state.AddStatus(eStatusEffect.INTANGIBLE, 1);
                TakeDamage(power);
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        if (!state.HandleDamage(damage))
        {
            ExecQueueFree();
        }
    }



    //ITarget
    public Vector2 GetTargetPosition()
    {
        if (IsInsideTree()) return Position;
        else return Vector2.Zero;
    }


    public bool IsFreed()
    {
        return weakref.GetRef() == null;
    }

    //IConductElectricity

    public void BecomeIntangible()
    {
        body.SetCollisionLayerBit((int)eCollisionLayers.ENTITY, false);
        body.SetCollisionLayerBit((int)eCollisionLayers.HOSTILE, false);
        body.SetCollisionLayerBit((int)eCollisionLayers.INTANGIBLE, true);
        sprite.Modulate = new SpriteTheme().cEnemyHit;
    }

    public void EndIntangible()
    {
        body.SetCollisionLayerBit((int)eCollisionLayers.ENTITY, true);
        body.SetCollisionLayerBit((int)eCollisionLayers.HOSTILE, true);
        body.SetCollisionLayerBit((int)eCollisionLayers.INTANGIBLE, false);
        sprite.Modulate = new SpriteTheme().cEnemy;
    }


    public void ExecQueueFree()
    {
        if (runtime.LeftTarget == this)
        {
            runtime.ClearLeftTarget();
        }

        if (runtime.RightTarget == this)
        {
            runtime.ClearRightTarget();
        }

        CallDeferred("free");
    }

    public void HandleCollision(KinematicCollision2D collision)
    {
        var collider = collision.GetCollider();

        if (collider is TileMap)
        {
            HandleTileCollision(collision);
        }
        if (collider is Wizard)
        {
            (collider as Wizard).Damage(GetDamage, eDamageType.PHYSICAL);
        }
    }

    private void HandleTileCollision(KinematicCollision2D collision)
    {
        var collider = collision.GetCollider() as TileMap;

        var level = runtime.World.GetLayer(collider);

        if ((int)level == (int)state.elevationHandler.Level + 1 && !state.isClimbing)
        {
            state.InitClimbing(level);
        }
    }

    public void CompleteClimb()
    {
        state.StopClimbing();
    }

    public void Elevate(eCollisionLayers level)
    {
        state.elevationHandler.HandleElevation((int)level);
    }

    public void DisableFall(int v)
    {

        state.DisableFall(v);
    }
    public void AddStatusEffect(IStatusEffect effect)
    {
        state.statusHandler.AddStatus(effect);
    }

    public bool HasStatusEffect(eStatusEffect e)
    {
        return state.statusHandler.HasStatus(e);
    }
    public void RemoveEffect(eStatusEffect eff)
    {
        state.statusHandler.RemoveStatus(eff);
    }

    public void AddJoltedEffect(int d)
    {
        state.AddStatus(eStatusEffect.JOLTED, d);
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


    private void OverrideSpriteColor()
    {
        var theme = new SpriteTheme();
        var defaultColor = theme.cEnemy;
        sprite.Modulate = state.statusHandler.HasStatus(eStatusEffect.INTANGIBLE) ? theme.cEnemyHit : defaultColor;
    }

}

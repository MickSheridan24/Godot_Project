using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Enemy : BaseActorNode, IElevatable, IMove, ITarget, IDamageable, IHaveRuntime,
                     IConductElectricity, IFreeable, ISufferStatusEffects, IHaveHealth, IHaveSize, ISelectable
{
    public string name { get; set; }
    public AnimationPlayer animation => sprite.GetNode<AnimationPlayer>("AnimationPlayer");
    public Vector2 speed { get; set; }
    public Area2D body => GetNode<Area2D>("Area2D");
    private int DamageStateCounter;

    private IOrder order;

    public Vector2 size => new Vector2(40, 40);
    public int Health => state.health.current;
    public int MaxHealth => state.health.standard;

    public int GetDamage => GetState().damage.current;
    public SelectionIndicator selectionIndicator => GetNode<SelectionIndicator>("SelectionIndicator");

    public string EntityName => "Enemy";

    public string Description => "Very Dangerous. Should Avoid";

    public override void _Ready()
    {
        destination = Position;
        isFallDisabled = false;
        speed = new Vector2(80, 80);
        moveable = new Moveable(this);
        MovingTarget = true;
        DamageStateCounter = 0;

        weakref = WeakRef(this);
    }

    public override void _Process(float d)
    {

        InitState();
        state.elevationHandler.HandleElevation();
        state.statusHandler.HandleStatuses();

        OverrideSpriteColor();

        selectionIndicator.ProcessSelection();
    }
    public override void _PhysicsProcess(float d)
    {
        order = GetState()?.RequestAction(d);
        state?.Tick();
        if (order != null)
        {
            order.Execute();
            order = null;
        }
    }

    public EnemyState GetState()
    {
        return state as EnemyState;
    }

    private void InitState()
    {
        if (state == null)
        {
            state = runtime.CreateEnemyState(this);
        }
    }

    public override void _UnhandledInput(InputEvent e)
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

        if ((int)level == (int)state.elevationHandler.Level + 1 && !GetState().isClimbing)
        {
            GetState().InitClimbing(level);
        }
    }

    public void CompleteClimb()
    {
        GetState().StopClimbing();
    }


    public void DisableFall(int v)
    {

        GetState().DisableFall(v);
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


    public void RightClick(InputEventMouseButton @event)
    {
        return;
    }

    public Rect2 GetSelectionArea()
    {
        return new Rect2(GlobalPosition - size / 2, size);
    }

    public bool CanMove()
    {
        return GetState().CanMove();
    }
}

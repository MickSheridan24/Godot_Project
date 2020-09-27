using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Enemy : KinematicBody2D, IMove, ITarget, IDamageable, IHaveRuntime, IConductElectricity, IFreeable, ISufferStatusEffects, IHaveHealth
{
    public string name { get; set; }
    public Sprite sprite => GetNode<Sprite>("Sprite");
    public Runtime runtime => GetParent<IHaveRuntime>().runtime;
    public EnemyState state { get; private set; }
    private Moveable moveable;
    public Vector2 destination { get; set; }
    public Vector2 speed => new Vector2(100, 100);
    public bool MovingTarget { get; set; }
    public Area2D body => GetNode<Area2D>("Area2D");
    private int DamageStateCounter;

    private IOrder order;
    private Highlight rightHighlight => GetNode<Highlight>("RightHighlight");
    private Highlight leftHighlight => GetNode<Highlight>("LeftHighlight");

    public int Health => state.health;
    public int MaxHealth => state.maxHealth;
    public override void _Ready()
    {
        destination = Position;
        sprite.Modulate = new SpriteTheme().cEnemy;
        moveable = new Moveable(this);
        MovingTarget = true;
        rightHighlight.position = Vector2.Zero;
        leftHighlight.position = Vector2.Zero;
        DamageStateCounter = 0;

        rightHighlight.color = new UITheme().cAccent;
        leftHighlight.color = new UITheme().cBlue;

    }


    public override void _Process(float d)
    {
        rightHighlight.Visible = runtime.RightTarget == this;
        leftHighlight.Visible = runtime.LeftTarget == this;
        InitState();
        state.HandleStatuses();
        HandleDamageCounter();
        order = state.RequestAction(d);
    }
    public override void _PhysicsProcess(float d)
    {
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

    private void HandleDamageCounter()
    {
        if (DamageStateCounter > 0)
        {

            DamageStateCounter--;
            if (DamageStateCounter <= 0)
            {
                body.SetCollisionLayerBit((int)eCollisionLayers.ENTITY, true);
                body.SetCollisionLayerBit((int)eCollisionLayers.HOSTILE, true);
                body.SetCollisionLayerBit((int)eCollisionLayers.INTANGIBLE, false);
                sprite.Modulate = new SpriteTheme().cEnemy;
            }

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
        MoveAndCollide(d * destination * speed);
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
                EnterDamageState(10);
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
        return Position;
    }


    //IConductElectricity

    public void EnterDamageState(int amount = 5)
    {
        body.SetCollisionLayerBit((int)eCollisionLayers.ENTITY, false);
        body.SetCollisionLayerBit((int)eCollisionLayers.HOSTILE, false);
        body.SetCollisionLayerBit((int)eCollisionLayers.INTANGIBLE, true);
        sprite.Modulate = new SpriteTheme().cEnemyHit;
        DamageStateCounter = amount;


    }

    public void AddStatusEffect(IStatusEffect effect)
    {
        state.AddStatusEffect(effect);
    }

    public bool HasStatusEffect(eStatusEffect e)
    {
        return state.HasStatus(e);
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


}

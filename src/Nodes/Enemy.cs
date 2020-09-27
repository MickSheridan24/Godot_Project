using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Enemy : Node2D, IMove, ITarget, IDamageable, IHaveRuntime, IConductElectricity, IFreeable, ISufferStatusEffects, IHaveHealth
{
    public string name { get; set; }
    public Sprite sprite => GetNode<Sprite>("Sprite");
    public Runtime runtime => GetParent<IHaveRuntime>().runtime;
    public EnemyState state { get; private set; }
    private Moveable moveable;
    public Vector2 destination { get; set; }
    public Vector2 speed => new Vector2(5, 5);
    public bool MovingTarget { get; set; }
    public Area2D body => GetNode<Area2D>("Area2D");
    private Highlight highlight => GetNode<Highlight>("Highlight");
    private int DamageStateCounter;

    public int Health => state.health;
    public int MaxHealth => state.maxHealth;
    public override void _Ready()
    {
        destination = Position;
        sprite.Modulate = new SpriteTheme().cEnemy;
        moveable = new Moveable(this);
        MovingTarget = true;
        highlight.position = Vector2.Zero;
        DamageStateCounter = 0;



    }



    public override void _Process(float d)
    {
        highlight.Visible = runtime.currentTarget == this;
        InitState();
        state.HandleStatuses();
        HandleDamageCounter();
        state.RequestAction().Execute();


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
        if (rClick?.ButtonIndex == (int)ButtonList.Right && rClick.IsPressed() &&
        runtime.currentSelection != null && !rClick.IsEcho() && pos.InBounds(Position - new Vector2(25, 25), Position + new Vector2(25, 25)))
        {
            runtime.SetTarget(this);
            GetTree().SetInputAsHandled();
        }
    }

    public void HandleMove()
    {
        moveable.HandleMove();
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
        if (runtime.currentTarget == this)
        {
            runtime.ClearTarget();
        }

        CallDeferred("free");
    }


}
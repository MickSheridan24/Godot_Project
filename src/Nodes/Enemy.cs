using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Enemy : BaseActorNode, IElevatable, IMove, ITarget, IDamageable, IHaveRuntime,
                     IConductElectricity, IFreeable, ISufferStatusEffects, IHaveHealth, IHaveSize, ISelectable,
                      ICanAttack, ICanFreeze
{
    public string name { get; set; }

    public Vector2 speed { get; set; }

    private int DamageStateCounter;

    private IOrder order;


    public eTeam Team => eTeam.HOSTILE;

    public Vector2 size => new Vector2(40, 40);
    public int Health => state.health.current;
    public int MaxHealth => state.health.standard;


    public int GetDamage => GetState().damage.current;
    public SelectionIndicator selectionIndicator => GetNode<SelectionIndicator>("SelectionIndicator");

    public string EntityName => "Enemy";

    public string Description => "Very Dangerous. Should Avoid";

    public TargetingSystem Targeting => state.Targeting;

    public float Range => 50;

    public int CurrentDamage => (state as EnemyState).damage.current;

    public eDamageType damageType => eDamageType.PHYSICAL;

    RayCast2D ICanAttack.RayCast => GetNode<RayCast2D>("RayCast");



    public override void _Ready()
    {
        destination = Position;
        isFallDisabled = false;
        speed = new Vector2(140, 140);
        moveable = new Moveable(this);
        MovingTarget = true;
        DamageStateCounter = 0;

        Model.Material = ((Material)Model.GetMaterial().Duplicate());

        weakref = WeakRef(this);
    }

    public override void _Process(float d)
    {
        base._Process(d);

        InitState();
        state.elevationHandler.HandleElevation();
        state.statusHandler.HandleStatuses();

        selectionIndicator.ProcessSelection();
        Update();
    }
    public override void _PhysicsProcess(float d)
    {
        if (!IsFreed())
        {
            base._PhysicsProcess(d);
            GetState()?.RequestAction(d);
            state?.Tick();
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
        if (!IsFreed())
        {
            if (!state.statusHandler.HasStatus(eStatusEffect.INTANGIBLE))
            {
                switch (type)
                {
                    default:
                        state.AddStatus(eStatusEffect.INTANGIBLE, 5);
                        TakeDamage(power);
                        break;
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (!state.HandleDamage(damage))
        {
            runtime.playerState.bank.knowledge++;
            runtime.playerState.bank.insight++;
            ExecQueueFree();
        }
    }



    //ITarget
    public Vector2 GetTargetPosition()
    {
        if (IsInsideTree()) return GlobalPosition;
        else return Vector2.Zero;
    }


    //IConductElectricity

    public void BecomeIntangible()
    {
        SetCollisionLayerBit((int)eCollisionLayers.ENTITY, false);
        SetCollisionLayerBit((int)eCollisionLayers.HOSTILE, false);
        SetCollisionLayerBit((int)eCollisionLayers.INTANGIBLE, true);
        Shade("isFlash", true);
    }

    public void EndIntangible()
    {
        SetCollisionLayerBit((int)eCollisionLayers.ENTITY, true);
        SetCollisionLayerBit((int)eCollisionLayers.HOSTILE, true);
        SetCollisionLayerBit((int)eCollisionLayers.INTANGIBLE, false);
        Shade("isFlash", false);
    }


    public void ExecQueueFree()
    {
        runtime.RemoveEntity(this);
        IsDead = true;

        //CallDeferred("free");
        QueueFree();
    }

    public void HandleCollision(KinematicCollision2D collision)
    {
        var collider = collision.Collider;

        if (collider is TileMap)
        {
            HandleTileCollision(collision);
        }
    }

    private void HandleTileCollision(KinematicCollision2D collision)
    {
        var collider = collision.Collider as TileMap;

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

    public PartialMenu GetPartial()
    {
        return null;
    }

    public IMenuState GetMenuState()
    {
        return null;
    }

    public ITask GetFriendlyTask(BaseActorNode node)
    {
        throw new NotImplementedException();
    }

    public ITask GetHostileTask(BaseActorNode node)
    {
        if (node is ICanAttack)
        {
            return new AttackTask(node as ICanAttack, this);
        }
        return null;
    }

    public void HighlightTarget()
    {
        Shade("isTargetedFoe", true);
    }

    public void DeHighlightTarget()
    {
        Shade("isTargetedFoe", false);
    }

    public Area2D GetTargetArea()
    {
        return GetNode<Area2D>("Attackable");
    }

    public void AddFreezingEffect(int dur)
    {
        state.statusHandler.AddStatus(new FreezingEffec)
    }
}

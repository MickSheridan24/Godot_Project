using Godot;
using System;

public abstract class NPC : BaseActorNode, ISelectable, IHaveHealth, IMove, IHaveRuntime, ICaster,
                      IElevatable, ITarget, IDamageable, ISufferStatusEffects, IHaveSize, IHaveTarget
{
    public string EntityName => state.Name;
    public string Description => state.Description;
    public int Health => state.health.current;
    public int MaxHealth => state.health.standard;

    public eTeam Team => eTeam.FRIENDLY;
    public Vector2 size => new Vector2(32, 32);
    public Vector2 speed => state.speed.current.ToVector();

    public SelectionIndicator selectionIndicator => GetNode<SelectionIndicator>("SelectionIndicator");

    public TargetingSystem Targeting => state.Targeting;

    public float Range => 50;

    public eDamageType damageType => eDamageType.PHYSICAL;

    public int CurrentDamage => (state as NPCState).damage.current;


    public override void _Ready()
    {
        base._Ready();
        OverrideModel();
    }

    public override void _Process(float d)
    {
        state.elevationHandler.HandleElevation();
        selectionIndicator.ProcessSelection();
    }

    protected virtual void OverrideModel()
    {
        return;
    }

    public void BecomeIntangible()
    {
        SetCollisionLayerBit((int)eCollisionLayers.ENTITY, false);
        SetCollisionLayerBit((int)eCollisionLayers.FRIENDLY, false);
        SetCollisionLayerBit((int)eCollisionLayers.INTANGIBLE, true);
        Shade("isFlash", true);
    }

    public void EndIntangible()
    {
        SetCollisionLayerBit((int)eCollisionLayers.ENTITY, true);
        SetCollisionLayerBit((int)eCollisionLayers.FRIENDLY, true);
        SetCollisionLayerBit((int)eCollisionLayers.INTANGIBLE, false);
        Shade("isFlash", false);
    }


    public void RightClick(InputEventMouseButton mouse)
    {
        if (!IsFreed())
        {
            var dest = GetGlobalMousePosition();
            SetDestination(dest);
        }
    }
    public Rect2 GetSelectionArea()
    {
        return new Rect2(GlobalPosition - size / 2, size);
    }


    public void TakeDamage(int damage)
    {
        if (!state.HandleDamage(damage))
        {
            ExecQueueFree();
        }
    }

    public void ExecQueueFree()
    {
        state.runtime.RemoveEntity(this);
        CallDeferred("queue_free");
    }

    public void RemoveEffect(eStatusEffect eff)
    {
        state.statusHandler.RemoveStatus(eff);
    }

    public Vector2 GetTargetPosition()
    {
        if (IsInsideTree()) return Position;
        else return Vector2.Zero;
    }

    public bool CanMove()
    {
        return true;
    }

    public void HandleCollision(KinematicCollision2D collision)
    {
        var collider = collision.Collider;

        if (collider is Enemy)
        {
            Damage((collider as Enemy).GetDamage, eDamageType.PHYSICAL);
        }
    }
    public void CompleteClimb()
    {
        return;
    }
    public void DisableFall(int v)
    {
        return;
    }

    public void Damage(int power, eDamageType type)
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

    public PartialMenu GetPartial()
    {
        return null;
    }

    public IMenuState GetMenuState()
    {
        return null;
    }

    public bool CanTarget(ITarget target)
    {
        return target is StructureNode || target is BaseActorNode;
    }

    public void SetLeftTarget(ITarget target)
    {
        Targeting.SetLeftTarget(target);
        SetTask(target);

    }

    public void SetRightTarget(ITarget target)
    {
        Targeting.SetRightTarget(target);
        SetTask(target);
    }

    private void SetTask(ITarget target)
    {
        SetDestination(target.GetTargetPosition());

        if (target.Team == Team)
        {
            var task = target.GetFriendlyTask(this);
            state.taskQueue.Add(task, "CURRENT");
        }
        else if (target.Team != eTeam.NEUTRAL)
        {
            var task = target.GetHostileTask(this);
            state.taskQueue.Add(task, "CURRENT");
        }

    }

    public void ClearTargets()
    {
        Targeting.Clear();
        state.taskQueue.Clear();
    }

    public virtual ITask GetFriendlyTask(BaseActorNode node)
    {
        return new DoNothingTask();
    }

    public virtual ITask GetHostileTask(BaseActorNode node)
    {
        if (node is ICanAttack)
        {
            return new AttackTask(node as ICanAttack, this);
        }
        return null;
    }
}

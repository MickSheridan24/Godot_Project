using Godot;
using System;

public class NPC : BaseActorNode, ISelectable, IHaveHealth, IMove, IHaveRuntime, ICaster,
                      IElevatable, ITarget, IDamageable, ISufferStatusEffects, IHaveSize
{
    public string EntityName => state.Name;
    public string Description => state.Description;
    public int Health => state.health.current;
    public int MaxHealth => state.health.standard;

    public Vector2 size => new Vector2(32, 32);
    public Vector2 speed => state.speed.current.ToVector();

    public SelectionIndicator selectionIndicator => GetNode<SelectionIndicator>("SelectionIndicator");

    public override void _Process(float d)
    {
        state.elevationHandler.HandleElevation();
        OverrideSpriteColor();
        selectionIndicator.ProcessSelection();
    }

    public void BecomeIntangible()
    {
        body.SetCollisionLayerBit((int)eCollisionLayers.ENTITY, false);
        body.SetCollisionLayerBit((int)eCollisionLayers.FRIENDLY, false);
        body.SetCollisionLayerBit((int)eCollisionLayers.INTANGIBLE, true);
        sprite.Modulate = new SpriteTheme().cEnemyHit;
    }

    public void EndIntangible()
    {
        body.SetCollisionLayerBit((int)eCollisionLayers.ENTITY, true);
        body.SetCollisionLayerBit((int)eCollisionLayers.FRIENDLY, true);
        body.SetCollisionLayerBit((int)eCollisionLayers.INTANGIBLE, false);
    }


    private void OverrideSpriteColor()
    {
        var defaultColor = theme.NPC;
        sprite.Modulate = state.statusHandler.HasStatus(eStatusEffect.INTANGIBLE) ? theme.cEnemyHit : defaultColor;
    }
    public void RightClick(InputEventMouseButton mouse)
    {
        var dest = GetGlobalMousePosition();
        SetDestination(dest);
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
        var collider = collision.GetCollider();

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
                    state.AddStatus(eStatusEffect.INTANGIBLE, 2);
                    TakeDamage(power);
                    break;
            }
        }
    }
}

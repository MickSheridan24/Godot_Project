using System.Runtime.InteropServices.ComTypes;
using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
public class StructureNode : Node2D, ISelectable, ITarget, IHaveRuntime,
 IHaveSize, IHaveSpawnArea, IHaveHealth, IDamageable, ISufferStatusEffects
{
    private WeakRef weakref;

    public IStructure state { get; set; }
    public StaticBody2D area => GetNode<StaticBody2D>("Collidable");
    public Runtime runtime => GetParent<IHaveRuntime>().runtime;
    public string EntityName => state.Name;
    public string Description => state.Description;
    public bool MovingTarget => false;

    public Area2D spawnArea => GetNode<Area2D>("SpawnArea");

    public eTeam Team { get; set; }
    public TargetingSystem Targeting { get => state.Targeting; }

    private Highlight rightHighlight => GetNode<Highlight>("RightHighlight");
    private Highlight leftHighlight => GetNode<Highlight>("LeftHighlight");
    private Highlight selectHighlight => GetNode<Highlight>("SelectHighlight");
    public virtual int Radius => 100;
    private Vector2 _size = new Vector2(160, 160);
    public Vector2 size { get => _size; set => _size = value; }

    private PackedScene snPartial => (PackedScene)ResourceLoader.Load("res://scenes/StructurePartialMenu.tscn");

    public Node2D Model => GetNode<Node2D>("Model");

    public int Health => state.Health;
    public int MaxHealth => state.MaxHealth;

    public override void _Ready()
    {
        weakref = WeakRef(this);
        rightHighlight.position = Vector2.Zero;
        leftHighlight.position = Vector2.Zero;
        rightHighlight.color = new UITheme().cAccent;
        leftHighlight.color = new UITheme().cBlue;
        selectHighlight.color = new UITheme().cPrimary;
    }

    public override void _Process(float d)
    {
        rightHighlight.Visible = runtime.RightTarget == this;
        leftHighlight.Visible = runtime.LeftTarget == this;

        selectHighlight.Visible = runtime.currentSelection == this;

        state.tickHandler.Tick();

    }


    public void Configure(eTeam team)
    {
        state.ConfigureNode(this);
        this.Team = team;
    }
    public bool GetCollisionLayerBit(int n)
    {
        return area.GetCollisionLayerBit(n);
    }

    public bool GetCollisionMaskBit(int n)
    {
        return area.GetCollisionMaskBit(n);
    }

    public Vector2 GetTargetPosition()
    {
        if (IsInsideTree()) return Position;
        else return Vector2.Zero;
    }

    public bool IsFreed()
    {
        return weakref.GetRef() == null;
    }

    public void RemoveCollisions()
    {
        area.SetCollisionLayerBit(0, false);
        area.SetCollisionMaskBit(0, false);
    }

    public void RightClick(InputEventMouseButton mouse)
    {
        state.RightClick(mouse.GlobalPosition);
    }

    public void Select()
    {
        runtime.SetSelection(this);
    }

    public void DeSelect()
    {
        return;
    }
    public void SetCollisionLayerBit(int n, bool v)
    {
        area.SetCollisionLayerBit(n, v);
    }

    public void SetCollisionMaskBit(int n, bool v)
    {
        area.SetCollisionMaskBit(n, v);
    }

    public Rect2 GetSelectionArea()
    {
        return new Rect2(GlobalPosition - size / 2, size);

    }

    public PartialMenu GetPartial()
    {
        return snPartial.Instance() as StructurePartialMenu;
    }

    public IMenuState GetMenuState()
    {
        return state as IMenuState;
    }

    public ITask GetFriendlyTask(BaseActorNode node)
    {
        return state.GetFriendlyTask(node, this);
    }

    public ITask GetHostileTask(BaseActorNode node)
    {
        return state.GetHostileTask(node, this);
    }

    public Vector2? FindOpenPosition(Vector2 nodeSize)
    {
        var obstacles = spawnArea.GetOverlappingAreas().ToList<Node>();

        var spawnVector = Position + (size / 2 * new Vector2(0, 1)) + new Vector2(0, 20);
        var direction = Vector2.Left;
        Node obstacle = (Node)obstacles.Find(o => ((Node)o).GetParent() is IHaveSize
                                                && ObstacleObstructs(((Node)o).GetParent<IHaveSize>(), spawnVector, nodeSize));
        int tries = 100;
        while (tries >= 0 && obstacle != null)
        {
            tries--;
            spawnVector = spawnVector + (new Vector2(10, 10) * direction);
            direction = ShiftSpawnVector(spawnVector, direction, obstacle.GetParent<IHaveSize>());
            obstacle = (Node)obstacles.Find(o => ((Node)o).GetParent() is IHaveSize
            && ObstacleObstructs(((Node)o).GetParent<IHaveSize>(), spawnVector, nodeSize));
        }
        if (obstacles.Find(o => ((Node)o).GetParent() is IHaveSize
                                && ObstacleObstructs(((Node)o).GetParent<IHaveSize>(), spawnVector, nodeSize)) == null)
        {
            return spawnVector;
        }
        return null;
    }

    private Vector2 ShiftSpawnVector(Vector2 vector, Vector2 direction, IHaveSize obs)
    {

        if (direction == Vector2.Left && vector.x + obs.size.x / 2 < Position.x - size.x / 2)
        {
            return Vector2.Up;
        }
        else if (direction == Vector2.Up && vector.y + obs.size.y / 2 < Position.y - size.y / 2)
        {
            return Vector2.Right;
        }
        else if (direction == Vector2.Right && vector.x - obs.size.x / 2 > Position.x + size.x / 2)
        {
            return Vector2.Down;
        }
        else if (direction == Vector2.Down && vector.y - obs.size.y / 2 > Position.y + size.y / 2)
        {
            return Vector2.Left;
        }
        return direction;
    }

    private bool ObstacleObstructs(IHaveSize obstacle, Vector2 vector, Vector2 size)
    {
        return new Rect2(obstacle.GlobalPosition, obstacle.size).Intersects(new Rect2(vector, size));
    }

    public void HighlightTarget()
    {

    }

    public void DeHighlightTarget()
    {

    }

    public void Damage(int power, eDamageType type)
    {
        TakeDamage(power);
    }

    public void TakeDamage(int amount)
    {
        if (!state.HandleDamage(amount))
        {
            ExecQueueFree();
        }
    }

    public void ExecQueueFree()
    {
        runtime.Tower = null;
        CallDeferred("queue_free");
    }

    public void BecomeIntangible()
    {

    }

    public void EndIntangible()
    {

    }

    public void RemoveEffect(eStatusEffect eff)
    {
        state.statusHandler.RemoveStatus(eff);
    }

    public Area2D GetTargetArea()
    {
        return GetNode<Area2D>("Attackable");
    }
}



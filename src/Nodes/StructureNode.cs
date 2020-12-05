using Godot;
using System;

public class StructureNode : Node2D, ISelectable, ITarget, IHaveRuntime, IHaveSize
{
    private WeakRef weakref;

    public IStructure state { get; set; }
    public Sprite sprite => GetNode<Sprite>("Sprite");
    public StaticBody2D area => GetNode<StaticBody2D>("Area2D");
    public Runtime runtime => GetParent<IHaveRuntime>().runtime;
    public string EntityName => state.Name;
    public string Description => state.Description;
    public bool MovingTarget => false;

    private Highlight rightHighlight => GetNode<Highlight>("RightHighlight");
    private Highlight leftHighlight => GetNode<Highlight>("LeftHighlight");
    private Highlight selectHighlight => GetNode<Highlight>("SelectHighlight");
    public Vector2 size => new Vector2(160, 160);

    private PackedScene snPartial => (PackedScene)ResourceLoader.Load("res://scenes/StructurePartialMenu.tscn");


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
    }

    public void Configure()
    {
        state.ConfigureNode(this);
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
        state.RightClick(mouse.Position);
    }

    public void Select()
    {
        runtime.SetSelection(this);
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
}



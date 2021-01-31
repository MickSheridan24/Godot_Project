using System;
using Godot;

public class BaseActorNode : KinematicBody2D
{

    public Runtime runtime => GetParent<IHaveRuntime>().runtime;
    public BaseActorState state { get; set; }



    //NodeInfo
    protected Sprite sprite => GetNode<Sprite>("Sprite");
    public Vector2 spritePosition => sprite.Position;
    protected WeakRef weakref;

    public SpriteTheme theme => new SpriteTheme();

    private bool active;


    public void Activate()
    {
        active = true;
        Visible = true;
    }



    // Moveable 
    public Moveable moveable;
    public Vector2 destination { get; set; }

    public bool MovingTarget { get; set; }



    //Elevateable

    public bool isFallDisabled { get; set; }


    public override void _Ready()
    {
        moveable = new Moveable(this as IMove);
        destination = Position;
        MovingTarget = true;
        isFallDisabled = false;
        weakref = WeakRef(this);
    }

    public override void _PhysicsProcess(float delta)
    {
        HandleMove(delta);
        state?.tickHandler.Tick();
    }


    public bool IsFreed()
    {
        return weakref.GetRef() == null;
    }


    // ISelectable
    public void Select()
    {
        runtime.currentSelection = this as ISelectable;
    }

    // IMoveable
    public void HandleMove(float d)
    {
        moveable.HandleMove(d);
    }

    //IElevateable 
    public void Elevate(eCollisionLayers level)
    {
        state.elevationHandler.HandleElevation((int)level);
    }


    //private

    protected void SetDestination(Vector2 position)
    {
        destination = position;
    }

}
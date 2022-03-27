using System;
using Godot;

public class BaseActorNode : KinematicBody2D
{

    public Runtime runtime => GetParent<IHaveRuntime>().runtime;
    public BaseActorState state { get; set; }

    protected bool IsDead { get; set; }
    //NodeInfo
    protected WeakRef weakref;

    public SpriteTheme theme => new SpriteTheme();

    private bool active;

    public KinematicBody2D Model => GetNode<KinematicBody2D>("Model");

    public CollisionShape2D ModelCollision => Model.HasNode("Collision") ? Model.GetNode<CollisionShape2D>("Collision") : null;
    public CollisionShape2D LocalCollision { get => GetNode<CollisionShape2D>("Collision"); set => AddChild(value); }


    public ShaderMaterial ModelMat => Model.Material as ShaderMaterial;


    public void Shade(string param, bool b)
    {
        ModelMat.SetShaderParam(param, b);
    }



    public bool debug { get; set; }


    public void Activate()
    {
        active = true;
        Visible = true;
    }



    // Moveable 
    public Moveable moveable;
    public Vector2 destination { get; set; }

    internal void InitiatePosition(Vector2 pos)
    {
        if (debug)
        {
            GD.Print("InitiatePostition");
        }
        GlobalPosition = pos;
        destination = pos;
    }

    public bool MovingTarget { get; set; }

    public int Radius => 60;

    //Elevateable

    public bool isFallDisabled { get; set; }
    public int EntityId { get; internal set; }

    public override void _Ready()
    {
        if (debug)
        {
            GD.Print("Debugging BaseActorNode");
        }
        moveable = new Moveable(this as IMove, debug);
        MovingTarget = true;
        isFallDisabled = false;
        weakref = WeakRef(this);
    }

    public override void _PhysicsProcess(float delta)
    {
        HandleMove(delta);
        state?.tickHandler.Tick();
        state.continuousActionHandler.Process();
        state.taskQueue.Process();
    }


    public bool IsFreed()
    {
        return IsDead || weakref?.GetRef() == null;
    }


    // ISelectable
    public void Select()
    {

        runtime.DeSelect();
        runtime.currentSelection = this as ISelectable;
        ModelMat.SetShaderParam("isSelected", true);
    }



    public void DeSelect()
    {
        ModelMat.SetShaderParam("isSelected", false);
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
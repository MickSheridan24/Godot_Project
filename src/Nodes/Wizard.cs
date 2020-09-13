using Godot;
using System;

public class Wizard : Node2D, ISelectable, IMove
{

    //props

    public Runtime runtime => GetParent<IHaveRuntime>().runtime;
    public WizardState state => runtime.WizardState;
    public Moveable moveable;
    private Sprite sprite => GetNode<Sprite>("Sprite");
    private Color unselected => new Color("#a822dd");
    private Color selected => new Color("a866ff");

    public Vector2 destination { get; set; }
    public Vector2 speed => state.moveSpeed;



    //overrides
    public override void _Ready()
    {
        moveable = new Moveable(this);
        destination = Position;
    }
    public override void _Process(float delta)
    {

        OverrideSpriteColor(runtime.currentSelection == this ? selected : unselected);

        HandleMove();
    }

    //signal handlers
    public void _onInputEvent(Node n, InputEvent @event, int idx)
    {
        if (@event is InputEventMouseButton && (@event as InputEventMouseButton).ButtonIndex == (int)ButtonList.Left)
        {
            GD.Print("CLICK");
            runtime.currentSelection = this;
        }
    }

    //ISelectable
    public void Select()
    {
        runtime.currentSelection = this;
    }
    public void RightClick(InputEventMouseButton mouse)
    {
        SetDestination(mouse.Position);
    }

    //IMove
    public void HandleMove()
    {
        moveable.HandleMove();
    }

    //private
    private void OverrideSpriteColor(Color c)
    {
        sprite.Modulate = c;
    }
    private void SetDestination(Vector2 position)
    {
        destination = position;
    }
}

using Godot;
using System;

public class SelectionIndicator : Node2D, IHaveSize
{


    private Highlight rightHighlight => GetNode<Highlight>("RightHighlight");
    private Highlight leftHighlight => GetNode<Highlight>("LeftHighlight");
    private Highlight selectHighlight => GetNode<Highlight>("SelectionHighlight");

    private Runtime runtime => GetParent<IHaveRuntime>().runtime;
    private ISelectable selectable => GetParent<ISelectable>();
    private ITarget targetable => GetParent<ITarget>();
    public Vector2 size => GetParent<IHaveSize>().size;

    public override void _Ready()
    {
        rightHighlight.color = new UITheme().cAccent;
        leftHighlight.color = new UITheme().cBlue;
        selectHighlight.color = new UITheme().cPrimary;
    }

    public void ProcessSelection()
    {
        rightHighlight.Visible = runtime.RightTarget == targetable;
        leftHighlight.Visible = runtime.LeftTarget == targetable;
        selectHighlight.Visible = runtime.IsSelected(selectable);
    }

}

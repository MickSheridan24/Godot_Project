using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class GroupSelection : ISelectable
{
    public List<ISelectable> group;

    public GroupSelection(List<ISelectable> group)
    {
        this.group = group;
    }

    public ISelectable first => group.FirstOrDefault();
    public Runtime runtime => first.runtime;

    public string EntityName => "Selection Group";

    public string Description => "Selected " + group.Count + " Units";

    public TargetingSystem Targeting => first.Targeting;

    public IMenuState GetMenuState()
    {
        return first.GetMenuState();
    }

    public PartialMenu GetPartial()
    {
        return first.GetPartial();
    }

    public Rect2 GetSelectionArea()
    {
        return first.GetSelectionArea();
    }

    public void RightClick(InputEventMouseButton @event)
    {
        foreach (var e in group)
        {
            e.RightClick(@event);
        }
    }

    public void Select()
    {
        foreach (var e in group)
        {
            e.Select();
        }
    }

    internal bool Has(ISelectable selectable)
    {
        return group.Any(s => s == selectable);
    }
}
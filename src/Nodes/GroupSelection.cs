using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class GroupSelection : ISelectable, IHaveTarget
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

    public bool CanTarget(ITarget target)
    {
        return GetTargeters().Any(t => t.CanTarget(target));
    }

    private List<IHaveTarget> GetTargeters()
    {
        return group.Where(e => e is IHaveTarget).Select(t => t as IHaveTarget).ToList();
    }

    public void ClearTargets()
    {
        foreach (var npc in GetTargeters())
        {
            npc.ClearTargets();
        }
    }

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

    public void SetLeftTarget(ITarget target)
    {
        foreach (var npc in GetTargeters().Where(t => t.CanTarget(target)))
        {
            npc.SetLeftTarget(target);
        }
    }

    public void SetRightTarget(ITarget target)
    {
        foreach (var npc in GetTargeters().Where(t => t.CanTarget(target)))
        {
            npc.SetRightTarget(target);
        }
    }

    internal bool Has(ISelectable selectable)
    {
        return group.Any(s => s == selectable);
    }
}
using System;
using Godot;
public interface ISelectable
{
    Runtime runtime { get; }

    void Select();
    void DeSelect();
    void RightClick(InputEventMouseButton @event);
    Rect2 GetSelectionArea();

    string EntityName { get; }
    string Description { get; }

    PartialMenu GetPartial();

    IMenuState GetMenuState();

    TargetingSystem Targeting { get; }
}
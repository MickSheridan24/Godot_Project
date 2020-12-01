using Godot;
public interface ISelectable
{
    Runtime runtime { get; }

    void Select();
    void RightClick(InputEventMouseButton @event);
    Rect2 GetSelectionArea();

    string EntityName { get; }
    string Description { get; }
}
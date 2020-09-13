using Godot;
public interface ISelectable
{
    Runtime runtime { get; }

    void Select();
    void RightClick(InputEventMouseButton mouse);

}
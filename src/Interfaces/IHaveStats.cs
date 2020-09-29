
using Godot;

public interface IHaveStats
{
    Stat speed { get; set; }
    TickHandler tickHandler { get; }
}


using Godot;

public interface IHaveStats
{
    Stat speed { get; set; }
    Stat health { get; set; }
    TickHandler tickHandler { get; }
}

using Godot;

namespace Mavisithor_Beaconizath.src.Interfaces
{
    public interface ICanDoLabor
    {
        eTeam Team { get; }

        BaseActorState state { get; }

        Vector2 Position { get; set; }
    }
}
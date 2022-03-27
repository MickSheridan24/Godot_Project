using Godot;
public interface IEnemySpawner
{

    World World { get; set; }

    Runtime Runtime { get; set; }

    bool IsExhausted(int Clock);

    void Spawn(Vector2 pos);

    bool CanSpawn(int Clock);

    EntityRegistry Registry { get; set; }
}
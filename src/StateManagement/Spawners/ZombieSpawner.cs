using Godot;

public class ZombieSpawner : IEnemySpawner
{

    private int Threshold = 1;
    private int TotalCount = 0;
    public World World { get; set; }
    public EntityRegistry Registry { get; set; }

    public Runtime Runtime { get; set; }
    private static PackedScene snZombie => (PackedScene)ResourceLoader.Load("res://scenes/Enemy.tscn");
    public bool CanSpawn(int Clock)
    {
        Threshold--;
        if (Threshold <= 0)
        {
            Threshold = 1;
            return true;
        }
        return false;
    }

    public bool IsExhausted(int Clock)
    {
        return TotalCount > 10;
    }

    public void Spawn(Vector2 pos)
    {
        var enemy = (Enemy)snZombie.Instance();
        enemy.GlobalPosition = pos;
        World.AddChild(enemy);

        var AI = new SmartZombieAI(enemy, Runtime.Tower, 1250);
        var state = new EnemyState(AI, enemy);
        AI.state = state;
        enemy.state = state;
        Registry.Add(enemy);
        TotalCount++;
    }
}

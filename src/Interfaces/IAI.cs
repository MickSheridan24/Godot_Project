using Godot;

public interface IAI
{
    IOrder Request(EnemyState enemyState, float d);

}
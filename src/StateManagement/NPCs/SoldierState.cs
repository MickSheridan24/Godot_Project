using Godot;

public class SoldierState : NPCState
{

    public SoldierState(Soldier node) : base(node)
    {
        speed = Stat.Speed(115);
        health = Stat.Health(1000);
        damage = Stat.Damage(100);
        range = Stat.Range(50);
    }


}

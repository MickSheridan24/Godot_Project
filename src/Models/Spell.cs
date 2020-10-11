using Godot;

public class Spell
{
    public string name { get; set; }
    public string text { get; set; }
    public eSpell type { get; set; }


    protected PackedScene snCircleHighlight => (PackedScene)ResourceLoader.Load("res://scenes/CircleHighlight.tscn");

}

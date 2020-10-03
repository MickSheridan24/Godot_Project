
using Godot;

public class TileChangeOrder
{
    public Vector2 coords { get; set; }
    public eTileType tileType { get; set; }
    public TileMap map { get; set; }


    public void Execute()
    {
        map.SetCellv(coords, (int)tileType);
    }
}

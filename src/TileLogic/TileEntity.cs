using Godot;

public class TileEntity
{
    public Vector2 coordsM { get; set; }
    public Vector2 coordsW { get; set; }
    public Color color { get; set; }
    public eTileType tileType { get; set; }

    public TileEntity(Vector2 coords, eTileType type, TileTheme theme)
    {
        coordsM = coords;
        coordsW = coords * coords.SPACE_SIZE();
        tileType = type;
        color = theme.GetColor(type);
    }


}

public enum eTileType
{
    DIRT = 1,
    GRASS = 2
}
using Godot;
public class TileTheme
{
    public Color cNeutral => new Color("#e5f1de");
    public Color cDirt => new Color("#a79358");
    public Color cGrass => new Color("#75c936");
    public Color cEarthWall => new Color("#8d7972");

    public Color GetColor(eTileType type)
    {
        switch (type)
        {
            case eTileType.DIRT:
                return cDirt;
            case eTileType.GRASS:
                return cGrass;
            case eTileType.EARTH_WALL:
                return cEarthWall;
            default:
                return cNeutral;
        }
    }
}
using Godot;
public class TileTheme
{
    public Color cNeutral => new Color("#e5f1de");
    public Color cDirt => new Color("#a79358");
    public Color cGrass => new Color("#75c936");

    public Color GetColor(eTileType type)
    {
        switch (type)
        {
            case eTileType.DIRT:
                return cDirt;
            case eTileType.GRASS:
                return cGrass;
            default:
                return cNeutral;
        }
    }
}
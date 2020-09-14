using Godot;
using System;
using System.Linq;

public class World : Node2D
{
    private TileMap tilemap => GetNode("TileMap") as TileMap;
    private TileSet tileSet => tilemap.Get("tile_set") as TileSet;
    public Runtime runtime => GetParent<IHaveRuntime>().runtime;
    private TileTheme theme;
    private MapHandler mapHandler;
    public override void _Ready()
    {
        mapHandler = new MapHandler();
        theme = new TileTheme();
        OverrideTileSet();
        mapHandler.GenerateTiles(50);
        mapHandler.AddGrass();
        mapHandler.tiles.ForEach(t => tilemap.SetCellv(t.coordsM, GetTileId(t.tileType)));
    }

    public override void _Process(float delta)
    {
        var globalPosition = GetGlobalMousePosition();
        var corner = tilemap.MapToWorld(tilemap.WorldToMap(globalPosition));

        runtime.hoveredCell = corner;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if ((@event as InputEventMouseButton)?.ButtonIndex == (int)ButtonList.Left
            && runtime.currentSelection != null)
        {
            runtime.currentSelection = null;
        }
    }

    private void OverrideTileSet()
    {
        tileSet.TileSetModulate(tileSet.FindTileByName("DIRT"), theme.cDirt);
        tileSet.TileSetModulate(tileSet.FindTileByName("GRASS"), theme.cGrass);
    }

    private int GetTileId(eTileType tileType)
    {
        var tile = tileSet.FindTileByName(tileType.ToString());
        return tile;
    }
}

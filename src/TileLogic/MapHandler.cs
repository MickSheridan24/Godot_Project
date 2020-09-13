using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
public class MapHandler
{
    private Random rng;
    private TileTheme theme;
    public List<TileEntity> tiles;
    private int size;

    public MapHandler()
    {
        rng = new Random();
        tiles = new List<TileEntity>();
        theme = new TileTheme();
    }

    public void GenerateTiles(int size)
    {
        this.size = size;
        for (int x = size * -1; x < size; x++)
        {
            for (int y = size * -1; y < size; y++)
            {
                GenerateTile(x, y, eTileType.DIRT);
            }
        }
    }

    private void GenerateTile(int x, int y, eTileType type)
    {
        var tile = new TileEntity(new Vector2(x, y), (eTileType)type, theme);
        tiles.Add(tile);
    }

    private void GenerateRandomTile(int x, int y)
    {
        var type = rng.Next(2) + 1;
        var tile = new TileEntity(new Vector2(x, y), (eTileType)type, theme);
        tiles.Add(tile);
    }

    public void AddGrass()
    {
        var fieldCount = Math.Ceiling((double)(size / 5));

        var origins = new List<Vector2>();

        for (var x = 0; x < fieldCount; x++)
        {
            origins.Add(new Vector2(size - rng.Next(size * 2), size - rng.Next(size * 2)));
        }

        origins.ForEach(o => AddGrassyField(o));
    }

    private void AddGrassyField(Vector2 o)
    {
        var grassyVectors = new List<Vector2>();

        grassyVectors.Add(o);

        grassyVectors.AddRange(GetRandomizedExpandedTerrain(o));

        var grassyTiles = grassyVectors.Select(v =>
        {
            return tiles.Find(t => t.coordsM == v);
        });

        foreach (var tile in grassyTiles)
        {
            tile.tileType = eTileType.GRASS;
        }
    }

    private List<Vector2> GetRandomizedExpandedTerrain(Vector2 o)
    {
        var retList = new List<Vector2>();

        var certainty = 10000;

        var current = o;
        while (certainty > 0)
        {
            if (rng.Next(10) < certainty)
            {
                var next = current + new Vector2(1 - rng.Next(3), 1 - rng.Next(3));
                if (next.InMap(size))
                {
                    retList.Add(next);
                    current = next;
                }
            }
            certainty -= 5;
        }
        return retList;
    }
}
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class World : Node2D, IHaveRuntime
{

    private TileMap level2 => GetNode("Level2") as TileMap;
    private TileSet L2tileset => level2.Get("tile_set") as TileSet;
    private TileMap level3 => GetNode("Level3") as TileMap;
    private TileSet L3tileset => level3.Get("tile_set") as TileSet;
    private TileMap level4 => GetNode("Level4") as TileMap;
    private TileSet L4tileset => level4.Get("tile_set") as TileSet;

    private List<TileMap> AllLevels;

    public Runtime runtime => GetParent<IHaveRuntime>().runtime;
    private TileTheme theme;
    private MapHandler mapHandler;
    public ITarget rightTarget => runtime.RightTarget;
    public ITarget leftTarget => runtime.LeftTarget;
    private Highlight rightHighlight => GetNode<Highlight>("RightHighlight");
    private Highlight leftHighlight => GetNode<Highlight>("LeftHighlight");

    public List<SimpleProjectile> projectileQueue { get; set; }
    private PackedScene snSimpleProjectile => (PackedScene)ResourceLoader.Load("res://scenes/SimpleProjectile.tscn");


    private bool update;
    public override void _Ready()
    {

        update = false;
        mapHandler = new MapHandler();
        theme = new TileTheme();
        projectileQueue = new List<SimpleProjectile>();



        AllLevels = new List<TileMap>()
        {
            level2, level3, level4
        };


        OverrideTileSet();
        mapHandler.GenerateTiles(50);
        mapHandler.AddGrass();
        mapHandler.tiles.ForEach(t => level3.SetCellv(t.coordsM, GetTileId(level3, t.tileType)));


    }


    public eCollisionLayers GetLayer(TileMap t)
    {
        return t == level2 ? eCollisionLayers.LEVEL2 :
               t == level3 ? eCollisionLayers.LEVEL3 :
               t == level4 ? eCollisionLayers.LEVEL4 :
               eCollisionLayers.ENTITY;
    }

    public override void _Process(float delta)
    {
        var globalPosition = GetGlobalMousePosition();

        if (update)
        {
            OverrideTileSet();
        }
        ConfigureHighlights();
        HandleProjectileSpawn();
    }


    public void CreateEarthWall(Vector2 v1)
    {
        var coord = level3.WorldToMap(v1);
        TryElevateTile(coord, level3, level4, eTileType.EARTH_WALL);
    }
    public void CreateEarthWall(Vector2 v1, Vector2 v2)
    {
        var v1Coords = level3.WorldToMap(v1);
        var v2Coords = level3.WorldToMap(v2);

        var coords = GetLineCoords(v1Coords, v2Coords);

        foreach (var coord in coords)
        {
            TryElevateTile(coord, level3, level4, eTileType.EARTH_WALL);
        }
    }

    public eCollisionLayers GetElevation(Vector2 v)
    {
        var coord = level3.WorldToMap(v);

        return level4.GetCellv(coord) != -1 ? eCollisionLayers.LEVEL4
             : level3.GetCellv(coord) != -1 ? eCollisionLayers.LEVEL3
             : eCollisionLayers.LEVEL2;
    }

    private void TryElevateTile(Vector2 coord, TileMap from, TileMap to, eTileType type)
    {
        var inFrom = from.GetCellv(coord);

        var inTo = to.GetCellv(coord);
        if (inFrom != -1 && inTo == -1)
        {
            to.SetCellv(coord, (int)eTileType.EARTH_WALL);
        }
    }

    public IEnumerable<Vector2> GetLineCoords(Vector2 v1, Vector2 v2)
    {
        var dir = v1.GetDirectionTo(v2);
        var ret = new List<Vector2>(){
            v1, v2
        };
        Vector2 primary;
        Vector2 alt;

        if (dir.Abs().x > dir.Abs().y)
        {
            primary = (dir * new Vector2(1, 0)).Normalized();
            alt = primary + (dir * new Vector2(0, 1)).Normalized();
        }
        else
        {
            primary = (dir * new Vector2(0, 1)).Normalized();
            alt = primary + (dir * new Vector2(1, 0)).Normalized();
        }

        var diff = (v1 - v2).Abs();
        var maxLength = diff.x > diff.y ? diff.x : diff.y;

        if (maxLength > 5)
        {
            maxLength = 5;
        }

        var dx = v1;
        for (var x = 0; x < maxLength; x++)
        {
            var dx1 = dx + primary;
            var dx2 = dx + alt;


            if (dx1.ProximityTo(v2) <= dx2.ProximityTo(v2))
            {
                ret.Add(dx1);
                dx = dx1;
            }
            else
            {
                ret.Add(dx2);
                dx = dx2;
            }
        }
        return ret.FillOutCoords();
    }

    private void OverrideTileSet()
    {
        foreach (var level in AllLevels)
        {
            level.TileSet.TileSetModulate(level.TileSet.FindTileByName("DIRT"), theme.cDirt);
            level.TileSet.TileSetModulate(level.TileSet.FindTileByName("GRASS"), theme.cGrass);
            level.TileSet.TileSetModulate(level.TileSet.FindTileByName("EARTH_WALL"), theme.cEarthWall);
        }

    }

    private int GetTileId(TileMap map, eTileType tileType)
    {
        var tile = map.TileSet.FindTileByName(tileType.ToString());
        return tile;
    }


    public void CreateProjectile(IProjectile projectileDetails, ICaster caster)
    {
        switch (projectileDetails.projectileType)
        {
            case eProjectileType.FIREBALL:
            case eProjectileType.LIGHTNING:
                CreateSimpleProjectile(projectileDetails, caster);
                break;
            default:
                break;
        }
    }

    private void CreateSimpleProjectile(IProjectile projectileDetails, ICaster caster)
    {
        var projectile = (SimpleProjectile)snSimpleProjectile.Instance();
        projectile.Config(projectileDetails, caster);
        projectileQueue.Add(projectile);
    }


    private void HandleProjectileSpawn()
    {
        if (projectileQueue.Count > 0)
        {
            foreach (var projectile in projectileQueue)
            {
                AddChild(projectile);
            }
        }
        projectileQueue = new List<SimpleProjectile>();
    }

    private void ConfigureHighlights()
    {
        rightHighlight.color = new UITheme().cAccent;
        leftHighlight.color = new UITheme().cBlue;

        rightHighlight.Visible = runtime?.RightTarget == rightTarget;

        if (rightHighlight.position != rightTarget?.GetTargetPosition())
        {
            rightHighlight.position = rightTarget?.GetTargetPosition() ?? Vector2.Zero;
            rightHighlight.Update();
        }

        leftHighlight.Visible = runtime?.LeftTarget == leftTarget;
        if (leftHighlight.position != leftTarget?.GetTargetPosition())
        {
            leftHighlight.position = leftTarget?.GetTargetPosition() ?? Vector2.Zero;
            leftHighlight.Update();
        }
    }
}

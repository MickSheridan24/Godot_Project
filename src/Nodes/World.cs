using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class World : Node2D, IHaveRuntime
{
    private TileMap tilemap => GetNode("TileMap") as TileMap;
    private TileSet tileSet => tilemap.Get("tile_set") as TileSet;
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
        OverrideTileSet();
        mapHandler.GenerateTiles(50);
        mapHandler.AddGrass();
        mapHandler.tiles.ForEach(t => tilemap.SetCellv(t.coordsM, GetTileId(t.tileType)));
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

    public void AlterTile(Vector2 pos, eTileType type)
    {
        var tileCoords = tilemap.WorldToMap(pos);
        tilemap.SetCellv(tileCoords, (int)type);
    }

    public void CreateTileLine(Vector2 v1, Vector2 v2, eTileType type)
    {
        var v1Coords = tilemap.WorldToMap(v1);
        var v2Coords = tilemap.WorldToMap(v2);

        var dir = v1Coords.GetDirectionTo(v2Coords);
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

        var diff = (v1Coords - v2Coords).Abs();
        var maxLength = diff.x > diff.y ? diff.x : diff.y;

        if (maxLength > 10)
        {
            maxLength = 10;
        }

        tilemap.SetCellv(v1Coords, (int)type);
        var dx = v1Coords;

        for (var x = 0; x < maxLength; x++)
        {
            var dx1 = dx + primary;
            var dx2 = dx + alt;

            if (dx1.ProximityTo(v2Coords) <= dx2.ProximityTo(v2Coords))
            {
                tilemap.SetCellv(dx1, (int)type);
                dx = dx1;
            }
            else
            {
                tilemap.SetCellv(dx2, (int)type);
                dx = dx2;
            }
        }
        tilemap.SetCellv(v2Coords, (int)type);
    }

    private void OverrideTileSet()
    {
        tileSet.TileSetModulate(tileSet.FindTileByName("DIRT"), theme.cDirt);
        tileSet.TileSetModulate(tileSet.FindTileByName("GRASS"), theme.cGrass);
        tileSet.TileSetModulate(tileSet.FindTileByName("EARTH_WALL"), theme.cEarthWall);
    }

    private int GetTileId(eTileType tileType)
    {
        var tile = tileSet.FindTileByName(tileType.ToString());
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

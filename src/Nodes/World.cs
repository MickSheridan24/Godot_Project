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

        var dir1 = v1Coords.DirectionTo(v2Coords).Normalized();
        var dir2 = dir1 * new Vector2(0, 1);
        var dir3 = dir2 * new Vector2(1, 0);

        var diff = (v1Coords - v2Coords).Abs();
        var maxLength = diff.x > diff.y ? diff.x : diff.y;


        tilemap.SetCellv(v1Coords, (int)type);
        var dx = v1Coords;

        for (var x = 0; x < maxLength; x++)
        {
            var dx1 = dx + dir1;
            var dx2 = dx + dir2;
            var dx3 = dx + dir3;

            if (dx1 >= dx2 && dx1 >= dx3)
            {
                tilemap.SetCellv(dx1, (int)type);
                dx = dx1;
            }
            else if (dx2 >= dx1 && dx2 >= dx3)
            {
                tilemap.SetCellv(dx2, (int)type);
                dx = dx2;
            }
            else if (dx3 >= dx1 && dx3 >= dx2)
            {
                tilemap.SetCellv(dx3, (int)type);
                dx = dx3;
            }
        }
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

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
    public ITarget targetPosition => runtime.currentTarget;
    private Highlight highlight => GetNode<Highlight>("Highlight");

    public List<SimpleProjectile> projectileQueue { get; set; }
    private PackedScene snSimpleProjectile => (PackedScene)ResourceLoader.Load("res://scenes/SimpleProjectile.tscn");

    public override void _Ready()
    {
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
        var corner = tilemap.MapToWorld(tilemap.WorldToMap(globalPosition));

        runtime.hoveredCell = corner;

        highlight.Visible = runtime?.currentTarget == targetPosition;

        if (highlight.position != targetPosition?.GetTargetPosition())
        {
            highlight.position = targetPosition?.GetTargetPosition() ?? Vector2.Zero;
            highlight.Update();
        }

        if (projectileQueue.Count > 0)
        {
            foreach (var projectile in projectileQueue)
            {
                AddChild(projectile);
            }
        }
        projectileQueue = new List<SimpleProjectile>();
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


    public void CreateProjectile(IProjectile projectileDetails, ICaster caster)
    {
        switch (projectileDetails.projectileType)
        {
            case eProjectileType.FIREBALL:
                CreateSimpleProjectile(projectileDetails, caster);
                break;
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
}

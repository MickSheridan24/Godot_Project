using Godot;

public class ProjectileBase
{
    public eProjectileType projectileType { get; set; }
    public Vector2 start { get; set; }
    public Vector2 direction { get; set; }
    public Vector2 speed { get; set; }
    public Vector2 range { get; set; }
    public IProjectileNode node { get; set; }

    public SpriteTheme theme => new SpriteTheme();

}
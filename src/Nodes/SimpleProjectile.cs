using Godot;
using System;

public class SimpleProjectile : Area2D
{
    private IProjectile details;

    public eProjectileType projectileType { get; private set; }
    public Vector2 speed => details.speed;
    public Vector2 direction => details.direction;
    public Vector2 maxDistance => details.maxDistance;
    public Sprite sprite => GetNode<Sprite>("Sprite");
    public SpriteTheme theme => new SpriteTheme();
    private Vector2 remainingDistance;

    public void Config(IProjectile projectileDetails)
    {
        Position = projectileDetails.start;
        details = projectileDetails;
        projectileType = projectileDetails.projectileType;
        remainingDistance = maxDistance;
        UpdateSprite();
    }

    public override void _Process(float d)
    {
        HandleMove();
    }

    private void UpdateSprite()
    {
        switch (details.projectileType)
        {
            case eProjectileType.FIREBALL:
                sprite.Set("modulate", theme.cFireball);
                break;
        }
    }
    private void HandleMove()
    {
        Position = Position + (direction * speed);
        remainingDistance -= speed;
        if (remainingDistance <= Vector2.Zero)
        {
            QueueFree();
        }
    }
}

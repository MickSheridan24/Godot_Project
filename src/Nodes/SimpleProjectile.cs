using Godot;
using System;

public class SimpleProjectile : Area2D
{
    private ProjectileEntity details;
    public Vector2 speed => details.speed;
    public Vector2 direction => details.direction;
    public Vector2 maxDistance => details.maxDistance;
    public Sprite sprite => GetNode<Sprite>("Sprite");
    public SpriteTheme theme => new SpriteTheme();
    private Vector2 remainingDistance;

    public override void _Process(float d)
    {
        HandleMove();
    }
    public void Config(Vector2 initPosition, ProjectileEntity projectileDetails)
    {
        Position = initPosition;
        details = projectileDetails;
        remainingDistance = maxDistance;
        UpdateSprite();
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

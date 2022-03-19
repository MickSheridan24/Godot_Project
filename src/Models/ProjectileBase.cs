using System.Collections.Generic;
using System.Linq;
using Godot;

public class ProjectileBase
{
    public eProjectileType projectileType { get; set; }
    public Vector2 start { get; set; }
    public Vector2 direction { get; set; }
    public Vector2 speed { get; set; }
    public Vector2 range { get; set; }

    public int damage { get; set; }
    public IProjectileNode node { get; set; }

    public SpriteTheme theme => new SpriteTheme();



    protected void TryDamage(Node target, eDamageType type)
    {
        if (target != null && target is IDamageable)
        {
            var damageable = target as IDamageable;
            damageable.Damage(damage, type);
        }
    }

    protected void SetEffectRadius(int r)
    {
        (node.effectRadius.GetNode<CollisionShape2D>("CollisionShape2D").Shape as CircleShape2D).Radius = r;
    }

    protected IEnumerable<Node> FindEffected<T>(Node exclude = null)
    {
        var areas = node.effectRadius.GetOverlappingAreas().ToList<Node>();
        return areas.Where(a => a != exclude && ((Node)a).GetParent() is T)
            .Select(t => ((Node)t).GetParent());
    }
}
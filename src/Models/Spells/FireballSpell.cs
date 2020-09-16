
using Godot;

public class FireballSpell : Spell, ISpell
{
    public FireballSpell()
    {
        name = "Cast Fireball";
        type = eSpell.FIREBALL;
        text = "FIREBALL";
    }
    public void Cast(Wizard caster, SpellTarget target)
    {
        var projectileDetails = new ProjectileEntity()
        {
            projectileType = eProjectileType.FIREBALL,
            speed = new Vector2(5, 5),
            direction = caster.position.DirectionTo(target.GetPosition()),
            maxDistance = new Vector2(200, 200)
        };
        caster.CreateProjectile(projectileDetails);
    }
}
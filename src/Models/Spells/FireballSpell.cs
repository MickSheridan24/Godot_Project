
using Godot;

public class FireballSpell : Spell, ISpell
{
    public FireballSpell()
    {
        name = "Cast Fireball";
        type = eSpell.FIREBALL;
        text = "FIREBALL";
    }
    public void Cast(Wizard caster)
    {
        var target = caster.runtime.RightTarget;
        var targetPos = target?.GetTargetPosition() ?? caster.spritePosition + new Vector2(1, 1);
        var projectileDetails = new FireballProjectile()
        {
            direction = caster.Position.GetDirectionTo(target.GetTargetPosition()),
            start = caster.spritePosition
        };
        caster.CreateProjectile(projectileDetails);
    }
}
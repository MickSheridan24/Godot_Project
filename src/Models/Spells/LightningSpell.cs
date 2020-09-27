using Godot;

public class LightningSpell : Spell, ISpell
{
    public LightningSpell()
    {
        name = "Cast Lightning";
        type = eSpell.LIGHTNING;
        text = "LIGHTNING BOLT";
    }
    public void Cast(Wizard caster)
    {
        var target = caster.runtime.RightTarget;
        var targetPos = target?.GetTargetPosition() ?? caster.spritePosition + new Vector2(1, 1);
        var projectileDetails = new LightningProjectile(20)
        {
            direction = caster.Position.GetDirectionTo(target.GetTargetPosition()),
            start = caster.spritePosition,
            damage = 100
        };
        caster.CreateProjectile(projectileDetails);
    }
}
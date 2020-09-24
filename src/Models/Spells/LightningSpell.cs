using Godot;

public class LightningSpell : Spell, ISpell
{
    public LightningSpell()
    {
        name = "Cast Lightning";
        type = eSpell.LIGHTNING;
        text = "LIGHTNING BOLT";
    }
    public void Cast(Wizard caster, ITarget target)
    {
        var targetPos = target?.GetTargetPosition() ?? caster.position + new Vector2(1, 1);
        var projectileDetails = new LightningProjectile()
        {
            direction = caster.Position.GetDirectionTo(target.GetTargetPosition()),
            start = caster.position
        };
        caster.CreateProjectile(projectileDetails);
    }
}
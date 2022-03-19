
using System.Collections.Generic;

public class FarmSpell : Spell, ISpell
{

	public static int effectRadius = 40;
	public FarmSpell()
	{
		name = "Grow Farm";
		type = eSpell.CREATE_FARM;
		text = "CORNSIDER THIS";
	}

	public void Cast(Wizard caster)
	{
		var target = caster.runtime.RightTarget;

		caster.runtime.World.CreateStructure(target.GetTargetPosition(), new Farm(caster.state.player), caster.Team);
	}
	public List<UIEffect> GetUIHints(Wizard caster)
	{
		var target = caster.runtime.RightTarget;

		var Circle = (CircleHighlight)snCircleHighlight.Instance();
		Circle.color = new UITheme().cGreen;
		Circle.radius = effectRadius;
		Circle.origin = caster.runtime.RightTarget;

		return new List<UIEffect>(){
			Circle
		};
	}
}

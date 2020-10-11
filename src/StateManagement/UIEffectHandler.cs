
using System;
using System.Collections.Generic;

public class UIEffectHandler
{
    public World world;
    public ISpell spell { get; set; }
    public List<UIEffect> effects { get; set; }


    public UIEffectHandler()
    {
        effects = new List<UIEffect>();
    }
    public void HintSpell(ISpell resSpell, Wizard wizardNode)
    {
        if (spell == null || resSpell.type != spell.type)
        {
            ChangeSpell(resSpell, wizardNode);
        }
    }

    public void CompleteCast()
    {
        spell = null;
        ClearEffects();
        effects = new List<UIEffect>();
    }

    private void ChangeSpell(ISpell resSpell, Wizard wizardNode)
    {
        spell = resSpell;
        ClearEffects();
        effects = spell.GetUIHints(wizardNode);
        world.AddEffects(effects);
    }

    private void ClearEffects()
    {
        world.ClearEffects(effects);
    }
}

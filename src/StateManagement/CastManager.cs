using System.Collections.Generic;
using System.Linq;

public class CastManager
{
    public string currentCast { get; set; }
    public WizardState wizard { get; set; }
    public ISpell currentSpell { get; set; }
    public List<ISpell> currentSpells { get; set; }

    public CastManager(WizardState wiz)
    {
        currentCast = "";
        wizard = wiz;
        currentSpells = wizard.GetAllUnlockedSpells();
        currentSpell = currentSpells.First();
    }

    public SpellCastResult UpdateCast(string s)
    {

        var proposedCast = currentCast + s;
        var success = false;
        var complete = false;

        var proposedSpells = NarrowSpells(proposedCast);

        if (proposedSpells.Count() > 0)
        {
            UpdateCurrentSpellState(proposedSpells, proposedCast);
            success = true;
            complete = currentSpell.text == currentCast;
        }

        return new SpellCastResult()
        {
            text = currentCast,
            Spell = currentSpell,
            success = success,
            complete = complete
        };
    }

    private void UpdateCurrentSpellState(IEnumerable<ISpell> proposedSpells, string proposedCast)
    {
        currentSpells = proposedSpells.ToList();
        currentSpell = currentSpells.Contains(currentSpell) ? currentSpell : currentSpells.First();
        currentCast = proposedCast;
    }

    private IEnumerable<ISpell> NarrowSpells(string proposedCast)
    {
        return currentSpells.Where(s => s.text.Contains(proposedCast));
    }

    private bool IsCastValid(string cast)
    {
        return currentSpell.text.Contains(cast) || currentSpells.Any(s => s.text.Contains(cast));
    }
}
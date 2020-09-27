
using System;
using System.Collections.Generic;
using Godot;
public class WizardState
{
    internal Vector2 moveSpeed;

    public List<ISpell> KnownSpells { get; set; }

    public WizardState()
    {
        moveSpeed = new Vector2(75, 75);
        KnownSpells = new List<ISpell>();

        KnownSpells.Add(new FireballSpell());
        KnownSpells.Add(new LightningSpell());
        KnownSpells.Add(new WallSpell());
    }
    internal List<ISpell> GetAllUnlockedSpells()
    {
        return KnownSpells;
    }
}

using System;
using System.Collections.Generic;
using Godot;
public class WizardState
{
    internal Vector2 moveSpeed;

    public List<ISpell> KnownSpells { get; set; }

    public WizardState()
    {
        moveSpeed = new Vector2(3, 3);
        KnownSpells = new List<ISpell>();

        KnownSpells.Add(new FireballSpell());
        KnownSpells.Add(new LightningSpell());
    }
    internal List<ISpell> GetAllUnlockedSpells()
    {
        return KnownSpells;
    }
}
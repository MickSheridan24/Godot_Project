
using System;
using System.Collections.Generic;
using Godot;
public class WizardState
{
    internal Vector2 moveSpeed;

    public List<ISpell> KnownSpells { get; set; }

    public WizardState()
    {
        moveSpeed = new Vector2(5, 5);
        KnownSpells = new List<ISpell>();

        KnownSpells.Add(new FireballSpell());
    }
    internal List<ISpell> GetAllUnlockedSpells()
    {
        return KnownSpells;
    }
}
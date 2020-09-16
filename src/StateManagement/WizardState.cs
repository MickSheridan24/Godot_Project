
using System;
using System.Collections.Generic;
using Godot;
public class WizardState
{
    internal Vector2 moveSpeed;

    public List<Spell> KnownSpells { get; set; }

    public WizardState()
    {
        moveSpeed = new Vector2(5, 5);
        KnownSpells = new List<Spell>();
        KnownSpells.Add(new Spell
        {
            text = "HELLO SPELLBOOK",
            name = "Test"
        });

        KnownSpells.Add(new Spell
        {
            text = "HELLOO SPELLBOOK",
            name = "Test2"
        });
    }

    internal List<Spell> GetAllUnlockedSpells()
    {
        return KnownSpells;
    }
}
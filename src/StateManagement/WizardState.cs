
using System;
using System.Collections.Generic;
using Godot;
public class WizardState
{
    internal Vector2 moveSpeed;

    public List<ISpell> KnownSpells { get; set; }

    public ElevationHandler elevationHandler;
    public Wizard node { get; set; }

    public WizardState(Wizard node)
    {
        this.node = node;
        moveSpeed = new Vector2(125, 125);
        KnownSpells = new List<ISpell>();

        KnownSpells.Add(new FireballSpell());
        KnownSpells.Add(new LightningSpell());
        KnownSpells.Add(new WallSpell());


        elevationHandler = new ElevationHandler(node, node.runtime);
    }
    internal List<ISpell> GetAllUnlockedSpells()
    {
        return KnownSpells;
    }
}
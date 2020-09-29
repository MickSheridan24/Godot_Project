
using System;
using System.Collections.Generic;
using Godot;
public class WizardState : IHaveStats
{
    public Stat speed { get; set; }

    public List<ISpell> KnownSpells { get; set; }

    public ElevationHandler elevationHandler;
    public Wizard node { get; set; }

    public TickHandler tickHandler { get; }
    public WizardState(Wizard node)
    {
        this.node = node;
        tickHandler = new TickHandler();
        speed = Stat.Speed(115);
        KnownSpells = new List<ISpell>();

        KnownSpells.Add(new FireballSpell());
        KnownSpells.Add(new LightningSpell());
        KnownSpells.Add(new WallSpell());
        KnownSpells.Add(new SpeedSpell());


        elevationHandler = new ElevationHandler(node, node.runtime);
    }
    internal List<ISpell> GetAllUnlockedSpells()
    {
        return KnownSpells;
    }
}
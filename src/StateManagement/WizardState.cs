
using System;
using System.Collections.Generic;
using Godot;

public class WizardState : BaseActorState, IHaveStats
{


    public List<ISpell> KnownSpells { get; set; }
    public WizardState(Wizard node) : base(node, node.runtime.playerState)
    {

        speed = Stat.Speed(115);
        health = Stat.Health(100);
        KnownSpells = new List<ISpell>();
        KnownSpells.Add(new FireballSpell());
        KnownSpells.Add(new LightningSpell());
        KnownSpells.Add(new WallSpell());
        KnownSpells.Add(new SpeedSpell());
        KnownSpells.Add(new GrassSpell());
        KnownSpells.Add(new FarmSpell());
    }
    public List<ISpell> GetAllUnlockedSpells()
    {
        return KnownSpells;
    }
}
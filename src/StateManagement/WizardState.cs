
using System;
using System.Collections.Generic;
using Godot;
public class WizardState : IHaveStats
{
    public Stat speed { get; set; }
    public Stat health { get; set; }

    public List<ISpell> KnownSpells { get; set; }

    public Wizard node { get; set; }

    public TickHandler tickHandler { get; }
    public StatusHandler statusHandler { get; private set; }
    public ElevationHandler elevationHandler { get; private set; }
    public WizardState(Wizard node)
    {
        this.node = node;
        tickHandler = new TickHandler();
        statusHandler = new StatusHandler(node);

        speed = Stat.Speed(115);
        health = Stat.Health(100);

        KnownSpells = new List<ISpell>();
        KnownSpells.Add(new FireballSpell());
        KnownSpells.Add(new LightningSpell());
        KnownSpells.Add(new WallSpell());
        KnownSpells.Add(new SpeedSpell());
        KnownSpells.Add(new GrassSpell());
        KnownSpells.Add(new FarmSpell());
        elevationHandler = new ElevationHandler(node, node.runtime);
    }
    public List<ISpell> GetAllUnlockedSpells()
    {
        return KnownSpells;
    }

    public bool HandleDamage(int damage)
    {
        health.current -= damage;
        return health.current > 0;
    }

    public void AddStatus(eStatusEffect s, int duration)
    {
        statusHandler.AddStatus(StatusEffect.Create(s));
        tickHandler.AddOrder(new RemoveStatusOrder(node, s), duration);
    }
}
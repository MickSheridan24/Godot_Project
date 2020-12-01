
using System.Collections.Generic;

public interface IWizardState
{
    Stat health { get; }
    Stat speed { get; }
    TickHandler tickHandler { get; }
    StatusHandler statusHandler { get; }
    ElevationHandler elevationHandler { get; }
    string Name { get; }
    string Description { get; }

    List<ISpell> GetAllUnlockedSpells();
    void AddStatus(eStatusEffect iNTANGIBLE, int v);
    bool HandleDamage(int damage);
    IResourceBank InitResourceBank();
}

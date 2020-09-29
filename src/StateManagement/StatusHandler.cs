using System.Collections.Generic;
using System.Linq;

public class StatusHandler
{
    private ISufferStatusEffects node;
    private List<IStatusEffect> statuses;

    public StatusHandler(ISufferStatusEffects node)
    {
        this.node = node;
        statuses = new List<IStatusEffect>();
    }


    public void HandleStatuses()
    {
        foreach (var status in statuses)
        {
            status.Enact(node);
        }
    }

    public IStatusEffect GetStatus(eStatusEffect effect)
    {
        return statuses.Where(s => s.type == effect).FirstOrDefault();
    }

    public bool HasStatus(eStatusEffect effect)
    {
        return GetStatus(effect) != null;
    }

    public void RemoveStatus(eStatusEffect effect)
    {
        var status = GetStatus(effect);
        if (status != null)
        {
            status.Reverse(node);
            statuses.Remove(status);
        }
    }

    public void AddStatus(IStatusEffect status)
    {
        if (!HasStatus(status.type))
        {
            statuses.Add(status);
        }
    }
}

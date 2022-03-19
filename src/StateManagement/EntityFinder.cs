using System.Collections.Generic;
using Godot;
using System.Linq;
public class EntityFinder
{

    public World world { get; set; }

    public EntityRegistry registry { get; set; }

    public EntityFinder(World world, EntityRegistry registry)
    {
        this.world = world;
        this.registry = registry;
    }
    public List<NPC> FindMinions(Vector2 origin, Vector2 target)
    {
        var ret = new List<NPC>();
        if (registry.NPCs.Count > 0)
        {
            for (var x = 0; x < registry.NPCs.Count; x++)
            {
                var e = registry.NPCs[x];
                if (!e.entity.IsFreed() && e.entity.Position > origin && e.entity.Position < target)
                {
                    ret.Add(e.entity as NPC);
                }
            }
            return ret;
        }
        return ret;
    }
}
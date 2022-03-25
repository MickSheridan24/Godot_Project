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
    public List<NPC> FindMinions(Vector2 origin, Vector2 range)
    {
        var ret = new List<NPC>();
        if (registry.NPCs.Count > 0)
        {
            for (var x = 0; x < registry.NPCs.Count; x++)
            {
                var e = registry.NPCs[x];

                var dir = origin.DirectionTo(e.entity.GlobalPosition);
                if (!e.entity.IsFreed() && e.entity.GlobalPosition.InBounds(origin, origin + range * dir))
                {
                    ret.Add(e.entity as NPC);
                }
            }
            return ret;
        }
        return ret;
    }
}
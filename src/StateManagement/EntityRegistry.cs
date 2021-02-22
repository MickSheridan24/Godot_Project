using System;
using System.Collections.Generic;

public class EntityRegistry
{
    public List<Entity> Entities { get; set; }
    public List<Entity> NPCs { get; set; }
    public List<Entity> Enemies { get; set; }
    public int currentId { get; set; }

    public EntityRegistry()
    {
        Entities = new List<Entity>();
        NPCs = new List<Entity>();
        Enemies = new List<Entity>();
        currentId = 1;
    }
    public void Add(BaseActorNode node)
    {
        var en = new Entity
        {
            id = currentId,
            entity = node,
            isMinion = node is NPC,
            isEnemy = node is Enemy
        };

        Entities.Add(en);
        if (en.isMinion)
        {
            NPCs.Add(en);
        }
        if (en.isEnemy)
        {
            Enemies.Add(en);
        }
        currentId++;
    }

    internal object Where(Func<object, object> p)
    {
        throw new NotImplementedException();
    }
}

public class Entity
{
    public int id { get; set; }
    public BaseActorNode entity { get; set; }
    public bool isMinion { get; set; }
    public bool isEnemy { get; set; }
}
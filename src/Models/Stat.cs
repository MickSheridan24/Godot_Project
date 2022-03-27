using System.Collections.Generic;

using System;

public class StatEffect
{
    public string name { get; set; }
    public int count { get; set; }
    public Func<int, int> effect { get; set; }
}

public class Stat
{
    public int standard { get; set; }
    public int current { get => CalcCurrent(); }

    private int flatDiff = 0;

    public Dictionary<string, StatEffect> effects { get; set; }
    public eStat type { get; set; }



    public Stat(int s, int c, eStat t)
    {
        standard = s;
        type = t;
        effects = new Dictionary<string, StatEffect>();
        AddEffect("flatDiff", (int i) => i + flatDiff, false);
    }

    public int CalcCurrent()
    {
        var ret = standard;
        foreach (var item in effects.Keys)
        {
            for (int x = 0; x < effects[item].count; x++)
            {
                ret = effects[item].effect(ret);
            }
        }
        return ret;
    }

    public void AddEffect(string name, Func<int, int> effect, bool stacks = false)
    {
        if (!effects.ContainsKey(name))
        {
            effects[name] = new StatEffect()
            {
                name = name,
                effect = effect,
                count = 1
            };
        }
        else if (stacks)
        {
            effects[name].count++;
        }
    }

    public void FlatChange(int amount)
    {
        flatDiff += amount;
        AddEffect("flatDiff", (int i) => i + flatDiff, false);
    }
    public void RemoveEffect(string name, bool removeStack = false)
    {
        if (effects.ContainsKey(name))
        {
            if (removeStack)
            {
                effects[name].count--;
                if (effects[name].count <= 0)
                {
                    effects.Remove(name);
                }
            }
            else effects.Remove(name);
        }
    }


    public static Stat Speed(int v)
    {
        return new Stat(v, v, eStat.SPEED);
    }

    public static Stat Health(int h)
    {
        return new Stat(h, h, eStat.HEALTH);
    }

    internal static Stat Damage(int v)
    {
        return new Stat(v, v, eStat.DAMAGE);
    }

    internal static Stat Range(int v)
    {
        return new Stat(v, v, eStat.RANGE);
    }
}

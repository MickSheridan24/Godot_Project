
using System;

public class Stat
{
    public int standard { get; set; }
    public int current { get; set; }
    public eStat type { get; set; }

    public Stat(int s, int c, eStat t)
    {
        standard = s;
        current = c;
        type = t;
    }
    public static Stat Speed(int v)
    {
        return new Stat(v, v, eStat.SPEED);
    }
}

using Godot;
public static class Extensions
{
    public static Vector2 SPACE_SIZE(this Vector2 v)
    {
        return new Vector2(32, 32);
    }

    public static Vector2 DirectionTo(this Vector2 vector, Vector2 dest)
    {
        return (dest - vector).Normalized();
    }

    public static bool WithinRange(this Vector2 pos, Vector2 dest, Vector2 range)
    {
        var diff = (pos.Abs() - dest.Abs()).Abs();
        return diff.x < range.x && diff.y < range.y;
    }

    public static bool InBounds(this Vector2 vector, Vector2 low, Vector2 high)
    {
        return vector.x >= low.x && vector.y >= low.y && vector.x < high.x && vector.y < high.y;
    }

    public static bool InMap(this Vector2 vector, int mapSize)
    {
        return vector.InBounds(new Vector2(-1 * mapSize, -1 * mapSize), new Vector2(mapSize, mapSize));
    }
}